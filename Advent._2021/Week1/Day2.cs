using Advent.Core;

namespace Advent._2021.Week1;

internal class Day2 : IReadInputDay<string[]>
{
    public void ReadData() => Input = File.ReadAllLines("Week1/Day2.txt");
    public string[] Input { get; set; }

    public object TaskA()
    {
        var (x, y) = Input
            .Select(s => s[0] switch
            {
                'f' => new Vec2(s[^1] - '0', 0),
                'd' => new Vec2(0, s[^1] - '0'),
                'u' => new Vec2(0, -s[^1] + '0')
            })
            .Aggregate((a, b) => a + b);
        return x * y;
    }

    public object TaskB()
    {
        var (x, y, _) = Input
            .Select(s => s[0] switch
            {
                'f' => new Vec3(s[^1] - '0', 0, 0),
                'd' => new Vec3(0, 0, s[^1] - '0'),
                'u' => new Vec3(0, 0, -s[^1] + '0')
            })
            .Aggregate((a, b) => a + (b.X, b.X * a.Z, b.Z));
        return x * y;
    }
}