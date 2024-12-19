namespace Advent._2024.Week3;

public class Day18(string[] input) : IDay
{
    private readonly Vec2 _goal = input.Length < 50 ? new Vec2(6, 6) : new Vec2(70, 70);
    private readonly List<Vec2> _obstacles = input.Select(ParseLine).ToList();
    
    public object Part1()
    {
        var size = input.Length < 50 ? 12 : 1024;
        var obstacles = _obstacles.Take(size).ToHashSet();

        return FindPathWithQueue(Vec2.Zero, _goal, obstacles);
    }

    public object Part2()
    {
        var start = 0;
        var end = input.Length;

        while (start <= end)
        {
            var mid = (start + end) / 2;
            var obstacles = _obstacles.Take(mid).ToHashSet();

            if (FindPathWithQueue(Vec2.Zero, _goal, obstacles) != -1)
            {
                start = mid + 1;
            }
            else
            {
                end = mid - 1;
            }
        }

        return input[end];
    }

    private int FindPathWithQueue(Vec2 start, Vec2 goal, HashSet<Vec2> obstacles)
    {
        var queue = new PriorityQueue<Vec2, int>();
        queue.Enqueue(start, 0);

        var visited = new int[_goal.Y + 1, _goal.X + 1];
        visited[start.Y, start.X] = 1;

        while (queue.TryDequeue(out var current, out var cost))
        {
            if (current == goal)
            {
                return cost;
            }

            foreach (var side in current.Sides())
            {
                if (side.IsInBounds(visited) && side.On(visited) == 0 && !obstacles.Contains(side))
                {
                    queue.Enqueue(side, cost + 1);
                    side.On(visited) = cost + 1;
                }
            }
        }

        return -1;
    }

    private static Vec2 ParseLine(string line)
    {
        var parts = line.Split(',');
        return new Vec2(int.Parse(parts[0]), int.Parse(parts[1]));
    }
}