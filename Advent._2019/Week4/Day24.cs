using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using MoreLinq;

namespace Advent._2019.Week4;

public static class Day24
{
    static int n = 5;
    public static int Map; 
    public static List<int> Maps = new List<int>();

    public static bool GetMap(ref int map, (int x, int y) p) => GetMap(ref map, p.x + 5 * p.y);
    public static bool GetMap(ref int map, int i) => (map & (1 << i)) != 0;
    public static void SetMap(ref int map, (int x, int y) p, bool v) => SetMap(ref map, p.x + 5 * p.y, v);
    public static void SetMap(ref int map, int i, bool v) => map = v ? map | (1 << i) : map & ~(1 << i);

    public static void Execute()
    {
        string[] input =
        {
            "#.#..",
            ".###.",
            "...#.",
            "###..",
            "#...."
        };

        input.SelectMany(x => x.Select(y => y == '#')).ForEach((x, y) => SetMap(ref Map, y, x));

        DrawMap();

        while (true)
        {
            if (Simulate())
                break;
#if DEBUG
            DrawMap();
#endif
        }

        var diversity = 0L;
        for (var i = 0; i < 25; i++)
            if (GetMap(ref Map, i))
                diversity += 1 << i;
            
        Console.WriteLine($"taskA: {diversity}");
    }

    public static bool Simulate()
    {
        if (Maps.Contains(Map))
        {
            Console.WriteLine("found same");
            return true;
        }

        Maps.Add(Map);

        var tempMap = Map;

        var adj = new (int x, int y)[] {(0, 1), (1, 0), (-1, 0), (0, -1)};

        for (int y = 0; y < n; y++)
        for (int x = 0; x < n; x++)
        {
            var p = (x, y);
            var bugs = 0;
            for (int j = 0; j < adj.Length; j++)
            {
                var pos = (x: p.x + adj[j].x, y: p.y + adj[j].y);
                if (pos.x >= 0 && pos.x < n
                               && pos.y >= 0 && pos.y < n
                               && GetMap(ref Map, pos))
                    bugs++;
            }

            if (GetMap(ref Map, p) && bugs != 1)
                SetMap(ref tempMap, p, false);

            if (!GetMap(ref Map, p) && (bugs == 1 || bugs == 2))
                SetMap(ref tempMap, p, true);
        }

        Map = tempMap;
        return false;
    }

    public static void DrawMap()
    {
        for (var y = 0; y < n; y++)
        {
            for (var x = 0; x < n; x++)
                Console.Write(GetMap(ref Map, (x, y)) ? "#" : ".");
            Console.WriteLine();
        }
        Console.WriteLine(Map);
    }
}