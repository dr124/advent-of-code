using Advent.Core;

namespace Advent._2021.Week1;

internal class Day5 : IReadInputDay
{
    private (Vec2 start, Vec2 end)[] Input;
    public void ReadData()
    {
        (Vec2, Vec2) ParseLine(string str)
        {
            var split = str.Split(new[] { ",", "->" }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(int.Parse)
                .ToArray();
            return (new Vec2(split[0], split[1]), new Vec2(split[2], split[3]));
        }

        Input = File.ReadAllLines("Week1/Day5.txt")
            .Select(ParseLine)
            .ToArray();
    }

    public object TaskA() => XD(Input.Where(x => x.start.X == x.end.X || x.start.Y == x.end.Y));

    public object TaskB() => XD(Input);

    int XD(IEnumerable<(Vec2 start, Vec2 end)> input)
    {
        var dict = new Dictionary<Vec2, int>();

        void Incr(Vec2 v)
        {
            if (dict.ContainsKey(v)) dict[v]++;
            else dict[v] = 1;
        }

        foreach (var v in input)
        {
            var dir = (v.end - v.start);
            if (dir.X != 0) dir.X /= Math.Abs(dir.X);
            if (dir.Y != 0) dir.Y /= Math.Abs(dir.Y);

            Incr(v.start);
            Incr(v.end);
            for (var vec = v.start + dir; vec != v.end; vec += dir) 
                Incr(vec);

        }
        return dict.Values.Count(x => x >= 2);
    }
}