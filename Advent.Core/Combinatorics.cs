using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent._2019
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

        public static List<List<T>> GetAllPossibleCombos<T>(List<List<T>> objects)
        {
            IEnumerable<List<T>> combos = new List<List<T>>() { new List<T>() };

            foreach (var inner in objects)
            {
                combos = combos.SelectMany(r => inner
                    .Select(x => {
                        var n = r.DeepClone();
                        if (x != null)
                        {
                            n.Add(x);
                        }
                        return n;
                    }).ToList());
            }

            // Remove combinations were all items are empty
            return combos.Where(c => c.Count > 0).ToList();
        }

        public static T DeepClone<T>(this T source)
        {
            // Don't serialize a null object, simply return the default for that object
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            var deserializeSettings = new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace };

            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source), deserializeSettings);

        }
    }
}