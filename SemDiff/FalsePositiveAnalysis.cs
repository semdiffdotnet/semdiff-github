using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SemDiff
{
    public class FalsePositiveAnalysis
    {
        private enum SpanDirection
        {
            Left,
            Right,
        }

        public void ThreeWayDiff(SyntaxTree ancestor, SyntaxTree lefttree, SyntaxTree righttree)
        {
            var intersections = GetIntersections(ancestor, lefttree, righttree);
        }

        private IEnumerable<SpanIntersection> GetIntersections(SyntaxTree ancestor, SyntaxTree lefttree, SyntaxTree righttree)
        {
            var leftchanges = lefttree.GetChanges(ancestor).Select(SpanHelper.Left).ToQueue();
            var rightchanges = righttree.GetChanges(ancestor).Select(SpanHelper.Right).ToQueue();

            var intersects = new HashSet<SpanIntersection>();
            var next = MinSpanStart(leftchanges, rightchanges);
            while (next != null)
            {
                var currentSpan = next.Dequeue();
                var intersection = new SpanIntersection();
                intersection.Add(currentSpan);
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

        private class SpanHelper
        {
            public TextChange Change { get; set; }
            public SpanDirection Dir { get; set; }
            public int End => Change.Span.End;
            public int Start => Change.Span.Start;

            public static SpanHelper Left(TextChange change) => new SpanHelper { Dir = SpanDirection.Left, Change = change };

            public static SpanHelper Right(TextChange change) => new SpanHelper { Dir = SpanDirection.Right, Change = change };
        }

        private class SpanIntersection
        {
            private List<SpanHelper> _changes = new List<SpanHelper>();
            public IEnumerable<SpanHelper> Changes => _changes;
            public int Count => _changes.Count;
            public int End { get; private set; } = int.MinValue;
            public IEnumerable<TextChange> Lefts => Changes.Where(c => c.Dir == SpanDirection.Left).Select(c => c.Change);
            public IEnumerable<TextChange> Rights => Changes.Where(c => c.Dir == SpanDirection.Left).Select(c => c.Change);
            public int Start { get; private set; } = int.MaxValue;

            public void Add(SpanHelper change)
            {
                _changes.Add(change);
                Start = Math.Min(Start, change.Start);
                End = Math.Max(End, change.End);
            }

            public override bool Equals(object obj) => _changes.Equals(obj);

            public override int GetHashCode() => _changes.GetHashCode();
        }
    }
}