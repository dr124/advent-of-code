using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace Advent._2024.Week2;

public class Day12(string[] input) : IDay
{
    private readonly HashSet<Vec2>[] _regions = GetRegions(input).ToArray();

    public object Part1() => _regions.Sum(CalculateFencePrice);

    public object Part2() => _regions.Sum(CalculateDiscountFencePrice);

    private int CalculateFencePrice(HashSet<Vec2> region)
    {
        var fences =
            from position in region
            from side in Vec2.Sides
            where !region.Contains(position + side)
            select position;

        return fences.Count() * region.Count;
    }

    private long CalculateDiscountFencePrice(HashSet<Vec2> region)
    {
        var fences = 0;
        var vecAlreadyWithFence = new HashSet<(Vec2 pos, Vec2 dir)>();
        foreach (var vec2 in region)
        {
            foreach (var side in Vec2.Sides)
            {
                if (!region.Contains(vec2 + side) && !vecAlreadyWithFence.Contains((vec2, side)))
                {
                    fences++;

                    for (var i = 0; ; i++)
                    {
                        var pIn = vec2 + side.Rotate(Rotation.Clockwise) * i;
                        if (!region.Contains(pIn) || region.Contains(pIn + side))
                        {
                            break;
                        }

                        vecAlreadyWithFence.Add((pIn, side));
                    }

                    for (var i = -1; ; i--)
                    {
                        var pIn = vec2 + side.Rotate(Rotation.Clockwise) * i;
                        if (!region.Contains(pIn) || region.Contains(pIn + side))
                        {
                            break;
                        }

                        vecAlreadyWithFence.Add((pIn, side));
                    }
                }
            }
        }

        return fences * region.Count;
    }

    private static IEnumerable<HashSet<Vec2>> GetRegions(string[] input)
    {
        Dictionary<Vec2, int> visited = [];
        Dictionary<int, HashSet<Vec2>> regions = [];

        void FloodFill(Vec2 pos, int id, char c)
        {
            visited[pos] = id;
            var region = new HashSet<Vec2> { pos };
            regions[id] = region;
            var queue = new Queue<Vec2>([pos]);

            while (queue.TryDequeue(out var v))
            {
                foreach (var side in Vec2.Sides)
                {
                    var vv = v + side;
                    if (vv.IsInBounds(input) && vv.On(input) == c && !region.Contains(vv))
                    {
                        visited[vv] = id;
                        region.Add(vv);
                        queue.Enqueue(vv);
                    }
                }
            }
        }

        for (var y = 0; y < input.Length; y++)
        {
            for (var x = 0; x < input[y].Length; x++)
            {
                Vec2 v = (x, y);
                if (!visited.ContainsKey(v))
                {
                    var id = regions.Count;
                    FloodFill(v, id, v.On(input));
                }
            }
        }

        return regions.Values;
    }
}