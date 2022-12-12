using Advent._2022.Week1;
using Advent.Core.Extensions;

namespace Advent._2022.Week2;

public class Day12 : IReadInputDay
{
    private Vec2 _s;
    private Vec2 _e;
    private int[,] _map;

    public void ReadData()
    {
        var xd = File.ReadAllLines("Week2/Day12.txt");

        var map = xd.To2dArray().ToMatrix();
        _s = map.EnumerateVec().First(p => map.At(p) == 'S');
        _e = map.EnumerateVec().First(p => map.At(p) == 'E');

        _map = xd.Select(x => x.Select(y => y - 'a'))
            .To2dArray()
            .ToMatrix();
        _map.At(_s) = 'a' - 'a';
        _map.At(_e) = 'z' - 'a';
    }

    public object? TaskA() => Process(false);

    public object? TaskB() => Process(true);

    private int Process(bool isTaskB)
    {
        var cost = _map.Copy();
        var toVisit = new Queue<Vec2>(new[] { _s });

        foreach (var v in cost.EnumerateVec())
        {
            if (_map.At(v) == 0 && isTaskB)
            {
                cost.At(v) = 0;
                toVisit.Enqueue(v);
            }
            else
            {
                cost.At(v) = 9999;
            }
        }

        cost.At(_s) = 0;
        cost.At(_e) = 9999;

        while (toVisit.TryDequeue(out var p))
        {
            foreach (var p2 in cost.Adjacent(p))
            {
                if (_map.At(p2) <= _map.At(p) + 1)
                {
                    if (cost.At(p2) > cost.At(p) + 1)
                    {
                        cost.At(p2) = cost.At(p) + 1;
                        toVisit.Enqueue(p2);
                    }
                }
            }
        }

        return cost.At(_e);
    }
}