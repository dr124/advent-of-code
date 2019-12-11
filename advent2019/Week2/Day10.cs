using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MoreLinq;

namespace advent2019.Week2
{
    public static class Day10
    {
        public static int[][] map;

        public static void Execute()
        {
            map = File.ReadAllLines(@"Week2\input10.txt")
                .Select(x => x.ToCharArray())
                .Select(x => x.Select(y => y == '.' ? 0 : -1).ToArray())
                .ToArray();

            var (a, b) = Process();
            Console.Write($"A: {a}, B: {b}");
        }

        public static (int, int) Process()
        {
            var bestPoint = GetAllPoints()
                .Select(x => new
                {
                    point = x,
                    visibleAsteroids = GetAllPoints()
                        .Except(new[] {x})
                        .Select(p => (x - p).AngleInDegrees)
                        .GroupBy(y => y)
                        .Count()
                }).MaxBy(x => x.visibleAsteroids)
                .First();

            var taskA = bestPoint.visibleAsteroids;
            var laser = bestPoint.point;

            var destroyedByLaser = GetAllPoints()
                .Except(new[] {laser})
                .Select(p => new
                {
                    p,
                    angle = (laser - p).Scale((-1, 1)).AngleInDegrees,
                    dist = (laser - p).Distance
                })
                .GroupBy(x => x.angle)
                .OrderBy(x => x.Key)
                .Select(x => x.OrderBy(y => y.dist)
                    .Select(y => y.p.X * 100 + y.p.Y)
                    .ToList())
                .ToList();

            var taskB = destroyedByLaser[199][0];

            return (taskA, taskB);
        }

        public static IEnumerable<Point> GetAllPoints()
        {
            for (var y = 0; y < map[0].Length; y++)
            for (var x = 0; x < map.Length; x++)
                if (MapPoint((x, y)) != 0)
                    yield return new Point(x, y);
        }

        public static ref int MapPoint(this Point p) => ref map[p.Y][p.X];
    }
}