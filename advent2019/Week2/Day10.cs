using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
            foreach (var p in GetAllPoints())
                MapPoint(p) = FindVisibleAsteroids(p);

            var taskA = map.Max(x => x.Max());

            var laser = GetAllPoints().First(p => MapPoint(p) == taskA);
            Console.WriteLine($"Best point: ({laser.X},{laser.Y})");

            var groupedPoints = GetAllPoints()
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

            var taskB = groupedPoints[199][0];

            return (taskA, taskB);
        }

        public static IEnumerable<Point> GetAllPoints()
        {
            for (var y = 0; y < map[0].Length; y++)
            for (var x = 0; x < map.Length; x++)
                if (MapPoint((x, y)) != 0)
                    yield return new Point(x, y);
        }

        public static int FindVisibleAsteroids(Point a)
        {
            return GetAllPoints()
                .Where(p => p != a)
                .Count(p => !GetCollidingAsteroids(a, p).Any());
        }

        public static IEnumerable<Point> GetCollidingAsteroids(Point A, Point B)
        {
            var d = B - A;
            return (d.X, d.Y) switch
            {
                (0, 0) => Enumerable.Empty<Point>(),
                (0, _) => GetCollidingY(A, B),
                (_, 0) => RayCollidesX(A, B),
                _ => GetCollidingDiagonal(A, B)
            };
        }

        public static IEnumerable<Point> RayCollidesX(Point A, Point B)
        {
            var dx = (B - A).X;
            var stepX = dx / Math.Abs(dx);

            for (var i = 1; i < Math.Abs(dx); i++)
            {
                var p = A + (i * stepX, 0);
                if (MapPoint(p) != 0)
                    yield return p;
            }
        }

        public static IEnumerable<Point> GetCollidingY(Point A, Point B)
        {
            var d = B - A;
            var stepY = d.Y / Math.Abs(d.Y);

            for (var i = 1; i < Math.Abs(d.Y); i++)
            {
                var p = A + (0, i * stepY);
                if (MapPoint(p) != 0)
                    yield return p;
            }
        }

        public static IEnumerable<Point> GetCollidingDiagonal(Point A, Point B)
        {
            var d = B - A;
            var stepY = d.X / Math.Abs(d.X);
            var dydx = (double) d.Y / d.X;

            var a = dydx;
            var b = A.Y - dydx * A.X;

            for (var i = 1; i <= Math.Abs(d.X); i++)
            {
                var x = A.X + stepY * i;
                var y = a * x + b;
                var ry = (int) Math.Round(y);
                var p = new Point(x, ry);

                if (Math.Abs(y - Math.Round(y)) <= 1e-5
                    && p != A && p != B
                    && MapPoint(p) != 0)
                    yield return p;
            }
        }

        public static ref int MapPoint(this Point p) => ref map[p.Y][p.X];
    }
}