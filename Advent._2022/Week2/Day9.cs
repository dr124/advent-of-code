using Advent.Core;
using Advent.Core.Extensions;

namespace Advent._2022.Week2;

public class Day9 : IReadInputDay
{
    private (char dir, int val)[] _input;
    
    public void ReadData()
    {
        _input = File.ReadLines("Week2/Day9.txt")
            .Select(line => (line[0], int.Parse(line[2..])))
            .ToArray();
    }

    public object TaskA() => SimulateLine(2);
    
    public object TaskB() => SimulateLine(10);

    private int SimulateLine(int size)
    {
        var visited = new HashSet<Vec2>();
        var line = new Vec2[size].Fill(Vec2.Zero);

        visited.Add(line[^1]);
        
        foreach (var (dir, val) in _input)
        {
            for (var i = 0; i < val; i++)
            {
                line[0] += Dir2Vec(dir);

                for (var l = 1; l < size; l++)
                {
                    TailFollowHead(ref line[l - 1], ref line[l]);
                }

                visited.Add(line[^1]);
            }
        }

        return visited.Count;
    }

    void TailFollowHead(ref Vec2 h, ref Vec2 t)
    {
        var d = h - t;

        if (Math.Abs(d.X) > 1 || Math.Abs(d.Y) > 1)
        {
            t += new Vec2(Math.Sign(d.X), Math.Sign(d.Y));
        }
    }

    private Vec2 Dir2Vec(char dir) => dir switch
    {
        'U' => (0, 1),
        'D' => (0, -1),
        'L' => (-1, 0),
        'R' => (1, 0),
    };
}