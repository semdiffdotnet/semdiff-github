using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SemDiff
{
    public static class Extensions
    {
        public static Queue<T> ToQueue<T>(this IEnumerable<T> source) => new Queue<T>(source);
        public static T Log<T>(this T obj, string format, params Func<T, object>[] selectors)
        {
            var formats = selectors.Select(selector =>
            {
                try
                {
                    return selector?.Invoke(obj).ToString();
                }
                catch (NullReferenceException)
                {
                    return null;
                }
            }).ToArray();
            var logMessage = string.Format("{0}: {1}", obj.GetType().Name, string.Format(format, formats));
            Trace.WriteLine(logMessage);
            return obj;
        }

        public static IList<T> ForEachLog<T>(this IList<T> obj, string format, params Func<T, object>[] selectors)
        {
            foreach(var item in obj)
            {
                item.Log(format, selectors);
            }
            return obj;
        }

        public static IEnumerable<T> ForEachLog<T>(this IEnumerable<T> obj, string format, params Func<T, object>[] selectors)
        {
            foreach (var item in obj)
            {
                item.Log(format, selectors);
            }
            return obj;
        }
    }
}