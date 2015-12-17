using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SemDiff
{
    public class FalsePositiveAnalysis
    {
        public enum SpanDirection
        {
            Left,
            Right,
        }

        public IEnumerable<Conflict> ThreeWayDiff(SyntaxTree ancestor, SyntaxTree lefttree, SyntaxTree righttree)
        {
            var leftchanges = lefttree.GetChanges(ancestor).ForEachLog("[LEFT] Span: {0}, NewText: '{1}'", t => t.Span, t => t.NewText);
            var rightchanges = righttree.GetChanges(ancestor).ForEachLog("[RIGHT] Span: {0}, NewText: '{1}'", t => t.Span, t => t.NewText); ;
            var intersects = GetIntersections(leftchanges, rightchanges).ForEachLog("[{0}..{1}), Count: {2}, Lefts: {3}, Rights: {4}", t => t.Start, t => t.End, t => t.Count, t => string.Join(" ", t.Lefts), t => string.Join(" ", t.Rights)); ;
            return GetConflicts(ancestor, lefttree, righttree, leftchanges, rightchanges, intersects);
        }

        private static IEnumerable<Conflict> GetConflicts(SyntaxTree ancestor, SyntaxTree lefttree, SyntaxTree righttree, IList<TextChange> leftchanges, IList<TextChange> rightchanges, IEnumerable<IntersectingChangedSpan> intersects)
        {
#if DEBUG
            //Insure that we have non-overlaping spans and that they are ordered
            int current_pointer = 0;
            Action<bool> assert = c =>
            {
                if (!c)
                    throw new InvalidOperationException("Overlapping Spans Detected");
            };
            foreach(var single_intersection in intersects)
            {
                assert(current_pointer <= single_intersection.Start);
                current_pointer = single_intersection.Start;
                assert(current_pointer <= single_intersection.End);
                current_pointer = single_intersection.End;
            }
#endif

            var conflicts = intersects.Select(i => new Conflict { Intersection = i }).ToList();

            var left = leftchanges.ToQueue();
            var right = rightchanges.ToQueue();

            //As changes are applied, we need to be able to map between the ancestor and the other files
            //these values will change as we move through the file and apply more and more changes.
            var lOffset = 0;
            var rOffset = 0;

            //There does not seem to be a way to index into the syntax tree for a particular index,
            //this way is easy, but in the future it may be found to be faster to manually walk the 
            //tree looking for the right indexes
            var ancestor_text = ancestor.ToString();
            var left_text = lefttree.ToString();
            var right_text = righttree.ToString();

            foreach (var conflict in conflicts)
            {
                //Changes before conflict
                lOffset += ApplyChangesUpTo(left, conflict.Intersection.Start);
                rOffset += ApplyChangesUpTo(right, conflict.Intersection.Start);

                //Changes within conflict
                var lInnnerOffset = ApplyChangesUpTo(left, conflict.Intersection.End, inclusive: true);
                var rInnerOffset = ApplyChangesUpTo(right, conflict.Intersection.End, inclusive: true);

                conflict.Ancestor = ConflictText.Create(ancestor, ancestor_text,
                    conflict.Intersection.Start, conflict.Intersection.Length);
                conflict.Left = ConflictText.Create(lefttree, left_text,
                    conflict.Intersection.Start + lOffset, conflict.Intersection.Length + lInnnerOffset);
                conflict.Right = ConflictText.Create(righttree, right_text,
                    conflict.Intersection.Start + rOffset, conflict.Intersection.Length + rInnerOffset);

                lOffset += lInnnerOffset;
                rOffset += rInnerOffset;
            }

            return conflicts;
        }

        private static int ApplyChangesUpTo(Queue<TextChange> changes, int end_of_changes, bool inclusive = false)
        {
            var offset = 0;
            while (changes.Count > 0 && (changes.Peek().Span.End < end_of_changes || (inclusive && changes.Peek().Span.End == end_of_changes)))
            {
                var s = changes.Dequeue();
                var rs = (TextChangeRange)s;
                offset += rs.NewLength - s.Span.Length;
            }

            return offset;
        }

        private IEnumerable<IntersectingChangedSpan> GetIntersections(IEnumerable<TextChange> left, IEnumerable<TextChange> right)
        {
            var leftchanges = left.Select(SpanHelper.Left).ToQueue();
            var rightchanges = right.Select(SpanHelper.Right).ToQueue();

            var intersects = new HashSet<IntersectingChangedSpan>();
            var next = MinSpanStart(leftchanges, rightchanges);
            while (next != null)
            {
                var intersection = new IntersectingChangedSpan();
                intersection.Add(next.Dequeue());
                var current = MinSpanStart(leftchanges, rightchanges);
                while (current != null && current.Peek().Start < intersection.End)
                {
                    intersection.Add(current.Dequeue());

                    if (!intersects.Contains(intersection))
                        intersects.Add(intersection); //Now that there are two, there is an intersection

                    current = MinSpanStart(leftchanges, rightchanges);
                }
                next = current;
            }

            return intersects;
        }

        private Queue<SpanHelper> MinSpanStart(Queue<SpanHelper> leftchanges, Queue<SpanHelper> rightchanges)
        {
            var lempty = leftchanges.Count == 0;
            var rempty = rightchanges.Count == 0;
            if (lempty && rempty)
            {
                return null;
            }
            else if (lempty)
            {
                return rightchanges;
            }
            else if (rempty)
            {
                return leftchanges;
            }
            var lstart = leftchanges.Peek().Start;
            var rstart = rightchanges.Peek().Start;

            if (lstart > rstart)
            {
                return rightchanges;
            }
            else
            {
                return leftchanges;
            }
        }

        public class Conflict
        {
            public IntersectingChangedSpan Intersection { get; set; }
            public ConflictText Left { get; set; }
            public ConflictText Right { get; set; }
            public ConflictText Ancestor { get; set; }

            public override string ToString()
            {
                var sb = new StringBuilder();
                sb.Append('<', 7);
                sb.Append(Left);
                sb.Append('|', 7);
                sb.Append(Ancestor);
                sb.Append('=', 7);
                sb.Append(Right);
                sb.Append('>', 7);
                return sb.ToString();
            }
        }

        public class SpanHelper
        {
            public TextChange Change { get; set; }
            public SpanDirection Dir { get; set; }
            public int End => Change.Span.End;
            public int Start => Change.Span.Start;

            public static SpanHelper Left(TextChange change) => new SpanHelper { Dir = SpanDirection.Left, Change = change };

            public static SpanHelper Right(TextChange change) => new SpanHelper { Dir = SpanDirection.Right, Change = change };
        }

        public class IntersectingChangedSpan
        {
            private List<SpanHelper> _changes = new List<SpanHelper>();
            public IEnumerable<SpanHelper> Changes => _changes;
            public int Count => _changes.Count;
            public IEnumerable<TextChange> Lefts => Changes.Where(c => c.Dir == SpanDirection.Left).Select(c => c.Change);
            public IEnumerable<TextChange> Rights => Changes.Where(c => c.Dir == SpanDirection.Right).Select(c => c.Change);
            public int Start { get; private set; } = int.MaxValue;
            public int End { get; private set; } = int.MinValue;
            public int Length => End - Start;

            public void Add(SpanHelper change)
            {
                _changes.Add(change);
                Start = Math.Min(Start, change.Start);
                End = Math.Max(End, change.End);
            }

            //public override bool Equals(object obj) => _changes.Equals(obj);

            //public override int GetHashCode() => _changes.GetHashCode();
        }

        public class ConflictText
        {
            public string Text { get; set; }
            public SyntaxTree Tree { get; set; }
            public int Start { get; set; }

            public override string ToString()
            {
                return Text;
            }

            internal static ConflictText Create(SyntaxTree tree, string text, int start, int length)
            {
                return new ConflictText
                {
                    Tree = tree,
                    Text = text.Substring(start, length),
                    Start = start,
                };
            }
        }
    }
}