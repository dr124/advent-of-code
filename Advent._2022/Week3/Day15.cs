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

    public object? TaskA()
    {
        int sum = 0;
        int y = 10;
        for (int x = 0;; x++)
        {
            if (_beacons.Contains((x,y))) 
                continue;

            if (!IsInRange((x, y)))
                break;
            
            sum++;
        }
        
        for (int x = -1;; x--)
        {
            if (_beacons.Contains((x,y))) 
                continue;

            if (!IsInRange((x, y)))
                break;
            
            sum++;
        }
        
        return sum;
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