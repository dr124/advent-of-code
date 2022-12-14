using Advent.Core.Extensions;

namespace Advent._2022.Week2;

public class Day14 : IReadInputDay
{
    private HashSet<Vec2> _map = new();
    private readonly Vec2 start = (500, 0);
    private readonly Vec2[] _m = { (0, 1), (-1, 1), (1, 1) };
    private int _minY = 0;

    public void ReadData()
    {
        var walls = File.ReadLines("Week2/Day14.txt")
            .Select(x => x.Split(" -> "))
            .Select(x => x.Select(Parse))
            .To2dArray();

        foreach (var wall in walls)
        {
            for (int i = 1; i < wall.Length; i++)
            {
                DrawBetweenPoints(wall[i - 1], wall[i]);
            }
        }

        Vec2 Parse(string s)
        {
            var coords = s.Split(",");
            return new Vec2(int.Parse(coords[0]), int.Parse(coords[1]));
        }

        void DrawBetweenPoints(Vec2 a, Vec2 b)
        {
            var d = b - a;
            var step = d / Math.Abs(d.X + d.Y);

            for (var p = a; p != b; p += step)
            {
                _map.Add(p);
            }

            _map.Add(a);
            _map.Add(b);
        }

        _minY = _map.Max(x => x.Y);
    }

    public object? TaskA()
    {
        var map = _map.ToHashSet();
        return Enumerate.From(1)
            .TakeWhile(_ => SimulateSand(map, false))
            .Last();
    }

    public object? TaskB()
    {
        var map = _map.ToHashSet();
        return Enumerate.From(1)
            .Do(_ => SimulateSand(map, true))
            .SkipWhile(_ => !map.Contains(start))
            .First();
    }

    private bool SimulateSand(ISet<Vec2> map, bool isB)
    {
        var s = start;

        while (s.Y <= _minY)
        {
            if (FallDown(s, map, out var r))
            {
                s = r;
            }
            else
            {
                map.Add(s);
                return true;
            }
        }

        if (isB)
        {
            map.Add(s);
        }

        return false;
    }

    private bool FallDown(Vec2 sand, ICollection<Vec2> map, out Vec2 newPosition)
    {
        foreach (var vec2 in _m)
        {
            if (!map.Contains(sand + vec2))
            {
                newPosition = sand + vec2;
                return true;
            }
        }

        newPosition = Vec2.Zero;
        return false;
    }
}
