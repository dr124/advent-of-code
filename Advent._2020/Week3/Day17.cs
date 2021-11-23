using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Advent.Core_2019_2020;
using MoreLinq;

//work in progress

namespace Advent._2020.Week3;

public class Day17 : Day<IEnumerable<(int x, int y)>, int>
{
    protected override IEnumerable<(int x, int y)> ReadData()
    {
        var lines = File.ReadAllLines("Week3/Input17.txt");
        for (var y = 0; y < lines.Length; y++)
        for (var x = 0; x < lines[y].Length; x++)
            if (lines[y][x] == '#')
                yield return (x, y);
    }

    protected override int TaskA()
    {
        var map2 = new Map<(int, int, int)>(Input.Select(o => (o.x, o.y, 0)));

        for (var i = 0; i < 5; i++)
            map2.Mutate();

        return map2.Mutate();
    }

    protected override int TaskB()
    {
        var map2 = new Map<(int, int, int, int)>(Input.Select(o => (o.x, o.y, 0, 0)));

        for (var i = 0; i < 5; i++)
            map2.Mutate();

        return map2.Mutate();
    }

    public class Map<T> where T : ITuple
    {
        public Map(IEnumerable<T> data) => HashMap = new HashSet<T>(data);

        public Map() { }

        public HashSet<T> HashMap { get; init; }
        public T Type => HashMap.First();
        public bool this[T pt]
        {
            get => HashMap.Contains(pt);
            set
            {
                if (HashMap.Contains(pt))
                {
                    if (!value)
                        HashMap.Remove(pt);
                }
                else
                {
                    if (value)
                        HashMap.Add(pt);
                }
            }
        }

        public int Mutate()
        {
            var prevMap = new Map<T> { HashMap = MoreEnumerable.ToHashSet(HashMap) };

            foreach (var pt in Enumerate())
            {
                var ne = prevMap.Neighbours(pt);
                this[pt] = prevMap[pt]
                    ? ne is 2 or 3
                    : ne is 3;
            }

            return HashMap.Count;
        }

        private IEnumerable<T> Enumerate()
        {
            var d = 5;
            var ranges = new (int min, int max)[Type.Length];
            for (int i = 0; i < Type.Length; i++)
                ranges[i] = ((int)HashMap.Min(x => x[i]) - d, (int)HashMap.Max(x => x[i]) + d);

            var range = ranges.Select(x => Enumerable.Range(x.min, x.max - x.min + 1).ToList()).ToList();

            foreach (var pts in Combinatorics.GetAllPossibleCombos(range))
                yield return CreatePoint(pts);
        }

        private int Neighbours(T pt)
        {
            var sum = 0;
            foreach (var ptt in Combinatorics.GetKCombsWithRept(new[] { -1, 0, 1 }, pt.Length))
            {
                if (ptt.All(y => y == 0))
                    continue;

                if (this[CreatePoint(ptt.Select((o, i) => (o + (int)pt[i])))])
                    sum += 1;
            }

            return sum;
        }

        private T CreatePoint(IEnumerable<int> pts) =>
            (T)Activator.CreateInstance(typeof(T), pts.Cast<object>().ToArray());
    }
}