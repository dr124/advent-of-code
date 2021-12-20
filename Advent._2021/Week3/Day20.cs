using System.Text;
using Advent.Core;
using Advent.Core.Extensions;

namespace Advent._2021.Week3;

internal class Day20 : IReadInputDay
{
    private string Algorithm;
    private Dictionary<Vec2, bool> Map = new();

    public void ReadData()
    {
        var lines = File.ReadAllLines("Week3/Day20.txt");
        Algorithm = lines[0];
        var map = lines[2..];

        for (int y = 0; y < map.Length; y++)
        for (int x = 0; x < map[y].Length; x++)
            Map[(x, y)] = map[y][x] == '#';
    }

    public object TaskA() => XD(Map.ToDictionary(x=>x.Key, x =>x.Value), 2);

    public object TaskB() => XD(Map.ToDictionary(x => x.Key, x => x.Value), 50);

    int XD(Dictionary<Vec2, bool> map, int maxSteps)
    {
        var e = maxSteps * 2;
        var minX = map.Min(x => x.Key.X);
        var maxX = map.Max(x => x.Key.X);
        var minY = map.Min(x => x.Key.Y);
        var maxY = map.Max(x => x.Key.Y);

        for (int steps = 0; steps < maxSteps; steps++)
        {
            Console.WriteLine(steps);
            var copy = map.ToDictionary(o => o.Key, o => o.Value);
            bool isPixel(Vec2 p) => copy.TryGetValue(p, out var b) && b;
            for (int x = minX - e; x <= maxX + e; x++)
            {
                for (int y = minY - e; y <= maxY + e; y++)
                {
                    var v = (Vec2)(x, y);
                    var num = 0;
                    if (isPixel(v + (1, 1))) num += 1 << 0;
                    if (isPixel(v + (0, 1))) num += 1 << 1;
                    if (isPixel(v + (-1, 1))) num += 1 << 2;
                    if (isPixel(v + (1, 0))) num += 1 << 3;
                    if (isPixel(v + (0, 0))) num += 1 << 4;
                    if (isPixel(v + (-1, 0))) num += 1 << 5;
                    if (isPixel(v + (1, -1))) num += 1 << 6;
                    if (isPixel(v + (0, -1))) num += 1 << 7;
                    if (isPixel(v + (-1, -1))) num += 1 << 8;
                    map[v] = Algorithm[num] == '#';
                }
            }
        }

        return map.Count(x => x.Key.X.IsBetween(minX - maxSteps, maxX + maxSteps)
                              && x.Key.Y.IsBetween(minY - maxSteps, maxY + maxSteps)
                              && x.Value);
    }
}