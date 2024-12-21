using System.Collections.Concurrent;

namespace Advent._2024.Week3;

public class Day20(string[] input) : IDay
{
    private readonly Vec2 _start = Extensions.ReadInput(input, 'S').First();
    private readonly HashSet<Vec2> _walls = Extensions.ReadInput(input, '#').ToHashSet();
    private Dictionary<Vec2, int>? _visited;
    
    public object Part1() => Calc(2, 100);

    public object Part2() => Calc(20, 100);

    private int Calc(int size, int threshold)
    {
        _visited ??= TraverseMaze(_start, _walls);

        var offsets = GetManhattanOffsets(size).ToList();
        return _visited
            .AsParallel()
            .SelectMany(vis1 => FindCheats(vis1, offsets, size, threshold))
            .Count() / 2;
    }

    private IEnumerable<int> FindCheats(KeyValuePair<Vec2, int> vis1, List<Vec2> offsets, int size, int threshold)
    {
        foreach (var offset in offsets)
        {
            var vis2 = vis1.Key + offset;
    
            if (!_visited.TryGetValue(vis2, out var cost2) || vis1.Key == vis2)
                continue;
    
            var dist = (vis1.Key - vis2).Manhattan();
            if (dist > size)
                continue;

            var diff = Math.Abs(vis1.Value - cost2) - dist;
            if (diff >= threshold)
            {
                yield return dist;
            }
        }
    }

    private static IEnumerable<Vec2> GetManhattanOffsets(int distance) =>
        from dx in Enumerable.Range(-distance, 2 * distance + 1)
        from dy in Enumerable.Range(-distance, 2 * distance + 1)
        where Math.Abs(dx) + Math.Abs(dy) <= distance
        select new Vec2(dx, dy);

    private static Dictionary<Vec2, int> TraverseMaze(Vec2 start, ISet<Vec2> walls)
    {
        var queue = new PriorityQueue<Vec2, int>();
        queue.Enqueue(start, 1);

        var visited = new Dictionary<Vec2, int>();
        visited[start] = 1;

        while (queue.TryDequeue(out var current, out var currentCost))
        {
            foreach (var side in current.Sides())
            {
                if (walls.Contains(side) || (visited.TryGetValue(side, out var existingCost) && existingCost <= currentCost + 1))
                    continue;

                visited[side] = currentCost + 1;
                queue.Enqueue(side, currentCost + 1);
            }
        }

        return visited;
    }
}