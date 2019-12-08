using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace advent2019
{
    public static class Utils
    {
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
            for (int i = r.Start.Value; i < r.End.Value; i++)
                yield return i;
        }
    }
}
