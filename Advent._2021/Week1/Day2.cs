using Advent.Core;

namespace Advent._2021.Week1;

internal class Day2 : IReadInputDay<Vec2[]>
{
    public Vec2[] Input { get; set; }
    public void ReadData() =>
        Input = File.ReadAllLines("Week1/Day2.txt")
            .Select(s => (Vec2)(s[0] switch
            {
                'f' => (s[^1] - '0', 0),
                'd' => (0, s[^1] - '0'),
                'u' => (0, -s[^1] + '0')
            })).ToArray();

    public object TaskA()
    {
        int x = 0, y = 0;
        foreach (var (dx, dy) in Input)
        {
            x += dx;
            y += dy;
        }

        return x * y; // 2036120
    }

    public object TaskB()
    {
        int x = 0, y = 0, a = 0;
        foreach (var (dx, da) in Input)
        {
            x += dx;
            y += dx * a;
            a += da;
        }

        return x * y; // 2015547716
    }
}