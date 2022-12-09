using System.Collections.Concurrent;
using System.Xml.Linq;
using Advent.Core;
using Advent.Core_2019_2020;

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

    private Vec2 Dir2Vec(char dir) => dir switch
    {
        'U' => new Vec2(0, 1),
        'D' => new Vec2(0, -1),
        'L' => new Vec2(-1, 0),
        'R' => new Vec2(1, 0),
    };


    public object TaskA() => SimulateLine(2);
    
    public object TaskB() => SimulateLine(10);

    void TailFollowHead(ref Vec2 h, ref Vec2 t)
    {
        var d = h - t;
        var absx = Math.Abs(d.X);
        var absy = Math.Abs(d.Y);
        var dirx = Math.Sign(d.X);
        var diry = Math.Sign(d.Y);
        if (absx == 1 && absy == 1)
        {
            // diagonal, it's good
        }
        else if (absx == 1 && absy == 0)
        {
            // horizontal, it's good
        }
        else if (absx == 0 && absy == 1)
        {
            // vertical, it's good
        }
        else
        {
            if (absx == 0) // move vertically
            {
                t += new Vec2(0, diry);
            }
            else if (absy == 0) // move horizontally
            {
                t += new Vec2(dirx, 0);
            }
            else // move diagonally
            {
                t += new Vec2(dirx, diry);
            }
        }
    }

    private int SimulateLine(int size)
    {
        var visited = new HashSet<Vec2>();
        var line = new Vec2[size].Populate(new Vec2(0,0));

        visited.Add(line[^1]);
        
        foreach (var (dir, val) in _input)
        {
            for (var i = 0; i < val; i++)
            {
                line[0] += Dir2Vec(dir);

                for (int l = 1; l < size; l++)
                {
                    TailFollowHead(ref line[l - 1], ref line[l]);
                }

                visited.Add(line[^1]);
            }
        }

        return visited.Count;
    }
}