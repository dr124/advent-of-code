using System.Numerics;
using Advent.Core;

namespace Advent._2022.Week3;

public class Day18 : IReadInputDay
{
    public HashSet<(int Y, int X, int Z)> _input;
    
    public void ReadData()
    {
        _input = File.ReadLines("Week3/Day18.txt")
            .Select(x => x.Split(",").Select(int.Parse).ToArray())
            .Select(x => (x[0], x[1], x[2]))
            .ToHashSet();
    }

    public object? TaskA() => SurfaceArea(_input);

    public object? TaskB()
    {
        var airPockets = new List<(int X, int Y, int Z)>();

        var minX = _input.Min(x => x.X);
        var maxX = _input.Max(x => x.X);
        var minY = _input.Min(x => x.Y);
        var maxY = _input.Max(x => x.Y);
        var minZ = _input.Min(x => x.Z);
        var maxZ = _input.Max(x => x.Z);

        for (int x = minX; x <= maxX; x++)
        for (int y = minY; y <= maxY; y++)
        for (int z = minZ; z <= maxZ; z++)
            if (_input.Contains((x, y, z))
                && _input.Contains((x + 1, y, z))
                && _input.Contains((x - 1, y, z))
                && _input.Contains((x, y + 1, z))
                && _input.Contains((x, y - 1, z))
                && _input.Contains((x, y, z + 1))
                && _input.Contains((x, y, z - 1)))
                airPockets.Add((x, y, z));


        return SurfaceArea(_input) - SurfaceArea(airPockets);
    }

    public int SurfaceArea(ICollection<(int X, int Y, int Z)> input)
    {
        var sides = new List<int>();
        foreach (var drop in input)
        {
            var s = 6;

            foreach (var drop2 in input)
            {
                if (drop == drop2) continue;

                var dx = drop.X - drop2.X;
                var dy = drop.Y - drop2.Y;
                var dz = drop.Z - drop2.Z;

                if (dx is 0 && dy is 0 && dz is 1 or -1)
                    s--;
                else if (dx is 0 && dy is 1 or -1 && dz is 0)
                    s--;
                else if (dx is 1 or -1 && dy is 0 && dz is 0)
                    s--;
            }

            sides.Add(s);
        }

        return sides.Sum();
    }
}