using Advent.Core;
using Advent.Core.Extensions;

namespace Advent._2021.Week2;

internal class Day13 : IReadInputDay
{
    private Vec2[] Input;
    private Vec2[] Folds;

    public void ReadData()
    {
        var lines = File.ReadAllLines("Week2/Day13.txt");
        var foldCount = lines.Count(x => x.StartsWith("fold"));
        Input = lines[..^(foldCount + 1)].Select(Vec2.FromString).ToArray();
        Folds = lines[^foldCount..]
            .Select(x => x[11..])
            .Select(str =>
            {
                var isX = str[0] == 'x';
                var num = int.Parse(str[2..]);
                return (Vec2)(isX ? num : 0, isX ? 0 : num);
            })
            .ToArray();
    }

    public object TaskA()
    {
        var input = Input.ToArray();
        Fold(ref input, Folds[..1]);
        return input.Distinct().Count();
    }


    public object TaskB()
    {
        var input = Input.ToArray();
        Fold(ref input, Folds);
        Print(input);
        return null;
    }

    void Fold(ref Vec2[] input, Vec2[] folds)
    {
        foreach (var f in folds)
        foreach (var v in input)
            if (f.X > 0 && v.X > f.X)
                v.X = 2 * f.X - v.X;
            else if (f.Y > 0 && v.Y > f.Y)
                v.Y = 2 * f.Y - v.Y;
    }

    void Print(Vec2[] input)
    {
        int maxX = input.Max(x => x.X) + 1, maxY = input.Max(x => x.Y) + 1;
        var tab = new int[maxY, maxX];
        foreach (var v in input)
            if (v.X < maxX && v.Y < maxY)
                tab[v.Y, v.X] = 1;
        Console.WriteLine(tab.ToMatrixString("").Replace("0", " ").Replace("1", "#"));
    }
}