using System.Collections.Generic;

namespace SemDiff
{
    public static class Extensions
    {
        public static Queue<T> ToQueue<T>(this IEnumerable<T> source) => new Queue<T>(source);
    }
}