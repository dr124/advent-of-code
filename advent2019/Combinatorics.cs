using System;
using System.Collections.Generic;
using System.Linq;

namespace advent2019
{
    public static class Combinatorics
    {
        public static IEnumerable<IEnumerable<int>> Permutations(this IEnumerable<int> values)
        {
            return values.Count() == 1
                ? new[] {values}
                : values.SelectMany(v =>
                    Permutations(values.Where(x => x != v)), (v, p) => p.Prepend(v));
        }

        public static IEnumerable<IEnumerable<T>> GetKCombsWithRept<T>(IEnumerable<T> list, int length)
            where T : IComparable
        {
            if (length == 1) return list.Select(t => new[] {t});
            return GetKCombsWithRept(list, length - 1)
                .SelectMany(t => list.Where(o => o.CompareTo(t.Last()) >= 0),
                    (t1, t2) => t1.Concat(new[] {t2}));
        }

        public static IEnumerable<IEnumerable<T>> GetKCombs<T>(IEnumerable<T> list, int length) 
            where T : IComparable<T>
        {
            if (length == 1) return list.Select(t => new[] {t});
            return GetKCombs(list, length - 1)
                .SelectMany(t => list.Where(o => o.CompareTo(t.Last()) > 0),
                    (t1, t2) => t1.Concat(new[] {t2}));
        }
    }
}