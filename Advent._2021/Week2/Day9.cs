using System.Resources;
using System.Text;
using Advent.Core;
using Microsoft.CodeAnalysis;

namespace Advent._2021.Week2;

internal class Day9 : IReadInputDay
{
    public int[][] Input { get; set; }

    public void ReadData()
    {
        Input = File.ReadAllLines("Week2/Day9.txt")
            .Select(x => x.Select(c => c - '0').ToArray())
            .ToArray();
    }

    public object TaskA() => FindLowerPoints().Select(x => Input[x.Y][x.X] + 1).Sum();

    IEnumerable<Vec2> FindLowerPoints()
    {
        for (var y = 0; y < Input.Length; y++)
        for (var x = 0; x < Input[y].Length; x++)
            if (GetAdjacent(y, x, Input.Length, Input[y].Length)
                .All(v => Input[y][x] < Input[y + v.Y][x + v.X]))
                yield return (x, y);
    }

    IEnumerable<Vec2> GetAdjacent(int y, int x, int maxY, int maxX)
    {
        if (y > 0) yield return new Vec2(0, -1);
        if (y < maxY - 1) yield return new Vec2(0, +1);

        if (x > 0) yield return new Vec2(-1, 0);
        if (x < maxX - 1) yield return new Vec2(+1, 0);
    }

    public object TaskB()
    {
        var tab = new int[Input.Length][];
        for (var y = 0; y < tab.Length; y++)
            tab[y] = new int [Input[y].Length];

        for (var y = 0; y < Input.Length; y++)
        for (var x = 0; x < Input[y].Length; x++)
            if (Input[y][x] == 9)
                tab[y][x] = -1;

        var pts = FindLowerPoints().ToArray();
        for (var i = 0; i < pts.Length; i++)
            tab[pts[i].Y][pts[i].X] = i + 1;

        for (var e = 0; e < 10; e++)
        for (var y = 0; y < Input.Length; y++)
        for (var x = 0; x < Input[0].Length; x++)
            foreach (var (dy, dx) in GetAdjacent(y, x, Input.Length, Input[0].Length))
                if (tab[y + dx][x + dy] == 0 && tab[y][x] > 0)
                    tab[y + dx][x + dy] = tab[y][x];

        print();

        return tab
            .SelectMany(x => x)
            .Where(x => x > 0)
            .GroupBy(x => x)
            .Select(x => x.Count())
            .OrderByDescending(x => x)
            .Take(3)
            .Aggregate(1, (acc, val) => acc * val);

        void print()
        {
            var sb = new StringBuilder();
            for (var y = 0; y < Input.Length; y++, sb.AppendLine())
            for (var x = 0; x < Input[0].Length; x++)
                sb.Append(tab[y][x] switch
                {
                    0 => " ",
                    -1 => "█",
                    _ => tab[y][x] % 10
                });
            Console.WriteLine(sb);
        }
    }
}
