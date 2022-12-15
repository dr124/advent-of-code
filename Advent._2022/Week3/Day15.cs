using Advent.Core;
using Advent.Core.Extensions;
using BenchmarkDotNet.Mathematics;

namespace Advent._2022.Week3;

public class Day15 : IReadInputDay
{
    private List<Vec2> _beacons = new();
    private List<Vec2> _sensors = new();
    public void ReadData()
    {
        var lines = File.ReadAllLines("Week3/Day15.txt")
            .Select(x => x.Split(new string[] { "=", ",", ":" }, StringSplitOptions.TrimEntries))
            .To2dArray();

        foreach (var line in lines)
        {
            _sensors.Add((int.Parse(line[1]), int.Parse(line[3])));
            _beacons.Add((int.Parse(line[5]), int.Parse(line[7])));
        }
    }

    IEnumerable<Vec2> Spiral( int X, int Y){
        int x,y,dx,dy;
        x = y = dx =0;
        dy = -1;
        int t = Math.Max(X,Y);
        int maxI = t*t;
        for(int i =0; i < maxI; i++){
            if ((-X/2 <= x) && (x <= X/2) && (-Y/2 <= y) && (y <= Y/2))
            {
                yield return (x, y);
            }
            if( (x == y) || ((x < 0) && (x == -y)) || ((x > 0) && (x == 1-y))){
                t = dx;
                dx = -dy;
                dy = t;
            }
            x += dx;
            y += dy;
        }
    }
    
    public object? TaskA()
    {
        var centerX = (int)_beacons.Select(x => x.X).Average();
        var centerY = (int)_beacons.Select(x => x.Y).Average();

        for (int r = 1;r < 2_000_000; r++)
        {
            for (int x = -r; x <= r; x++)
            {
                for (int y = x - r; y <= r - x; y++)
                {
                    if(Math.Abs(x) + Math.Abs(y) != r)
                        continue;

                    Vec2 p = (x + centerX, y + centerY);
                    var res = IsNotInRange(p);
                    if (res)
                    {
                        return p.X * 4_000_000 + p.Y;
                    }
                }
                
            }
            Console.WriteLine($"r : {r}");
        }

        return -1;
    }
    
    bool IsInRange(Vec2 p)
    {
        for(int i = 0; i < _sensors.Count; i++)
        {
            var s = _sensors[i];
            var b = _beacons[i];
            var l = ManhattanDistance(s, b);
            var lps = ManhattanDistance(p, s);
            if (lps <= l)
            {
                return true;
            }
        }

        return false;
    }
    
    bool IsNotInRange(Vec2 p)
    {
        for(int i = 0; i < _sensors.Count; i++)
        {
            var s = _sensors[i];
            var b = _beacons[i];
            var l = ManhattanDistance(s, b);
            var lps = ManhattanDistance(p, s);
            if (lps <= l)
            {
                return false;
            }
        }

        return true;
    }

    private int ManhattanDistance(Vec2 v1, Vec2 v2)
    {
        return Math.Abs(v1.X - v2.X) + Math.Abs(v1.Y - v2.Y);
    }

    public object? TaskB()
    {
        return null;
    }
}

// 5186842 too high