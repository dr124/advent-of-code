using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Advent.Core;
using Advent.Core.Graphs;
using MoreLinq;

namespace Advent._2020.Week3
{
    public class Day21 : Day
    {
        protected override void Execute()
        {
            var lines = File.ReadAllLines("Week3/Input21.txt")
                .Select(x => x.Replace("peanuts", "peanoots")) // lol topkek
                .Select(x => x.Replace("shellfish", "shellfeesh")) // 🤷🏿‍
                .ToArray();

            var allergens = lines
                .Where(x => x.Contains("contains"))
                .SelectMany(x => x[x.IndexOf("contains")..].Remove(@"[,\(\)]").Split(" ")[1..])
                .Distinct()
                .ToArray();

            var allergensWithIngr = allergens
                .Select(a => (a, i: lines.Where(x => x.Contains(a)).Select(x => x.Remove(@"\(.*$"))));

            var allergensFiltered = allergensWithIngr.Select(x =>
                {
                    var split = x.i.Select(y => y.SplitClear(" "));
                    var common = FindCommon(split).ToArray();
                    return (x.a, common);
                })
                .ToDictionary(x => x.a, x => x.common);
            
            var ingrd = allergensFiltered.Values.SelectMany(x => x).Distinct().ToArray();
            
            // A:
            var possibleAllergens = allergensFiltered.SelectMany(x => x.Value).Distinct();
            var a = lines.Select(x => x.Remove(@"\(.*$"))
                .ToDelimitedString(" ")
                .SplitClear(" ")
                .Count(x => !possibleAllergens.Contains(x));

            //B: 
            var M = allergens.Length;
            var N = ingrd.Length;
            var graph = new int[M, N];
            for (var i = 0; i < M; i++)
            for (var j = 0; j < N; j++)
                graph[i, j] = allergensFiltered[allergens[i]].Contains(ingrd[j]) ? 1 : 0;

            var matching = new Matching();
            matching.MaxFlow(graph);

            var b = matching.Assigned
                .Select((x, i) => (x, a: allergens[x], i: ingrd[i]))
                .OrderBy(x => x.a)
                .Select(x => x.i)
                .ToDelimitedString(",");

            Console.WriteLine($"A: {a}");
            Console.WriteLine($"B: {b}");
        }

        public static IEnumerable<T> FindCommon<T>(IEnumerable<IEnumerable<T>> lists)
        {
            return lists.SelectMany(x => x).Distinct()
                .Where(x => lists.Select(y => y.Contains(x) ? 1 : 0)
                    .Sum() == lists.Count());
        }
    }
}