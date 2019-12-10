using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;

namespace advent2019
{
    public static class Utils
    {
        public static Point Point(this (int x, int y) v) => new Point(v.x,v.y);

        public static void EnqueueMany<T>(this Queue<T> queue, IEnumerable<T> value)
        {
            foreach (var v in value) 
                queue.Enqueue(v);
        }

        public static IEnumerable<IEnumerable<int>> Permutations(this IEnumerable<int> values)
        {
            return values.Count() == 1 ?
                new[] { values }
                : values.SelectMany(v =>
                    Permutations(values.Where(x => x != v)), (v, p) => p.Prepend(v));
        }

        public static IEnumerable<int> To(this Range r)
        {
            for (var i = r.Start.Value; i < r.End.Value; i++)
                yield return i;
        }
    }
}
