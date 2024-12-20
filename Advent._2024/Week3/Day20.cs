namespace Advent._2024.Week3;

public class Day20(string[] input) : IDay
{
    private Vec2 _start = Extensions.ReadInput(input, 'S').First();
    private Vec2 _end = Extensions.ReadInput(input, 'E').First();
    private HashSet<Vec2> _walls = Extensions.ReadInput(input, '#').ToHashSet();
    
    public object Part1()
    {
        var queue = new PriorityQueue<Vec2, int>();
        queue.Enqueue(_start, 1);

        var visited = new Dictionary<Vec2, int>();

        while (queue.TryDequeue(out var vec, out var cost))
        {
            if (vec == _end)
            {
                continue;
            }

            var sides = vec.Sides().AsEnumerable();
            sides = sides.Where(x => !_walls.Contains(x));
            sides = sides.Where(x => !visited.ContainsKey(x) || visited[x] > cost + 1);
            foreach (var side in sides)
            {
                visited[side] = cost + 1;
                queue.Enqueue(side, cost + 1);
            }
        }

        Dictionary<int, int> cheats = [];
        
        foreach (var wall in _walls)
        {
            var sides = wall.Sides().AsEnumerable();
            sides = sides.Where(x => !_walls.Contains(x));
            sides = sides.Where(x => visited.ContainsKey(x));
            var s = sides.ToList();

            if (s.Count >= 2)
            {
                var pairs = s.Skip(1).Zip(s).ToList();
                foreach (var pair in pairs)
                {
                    var cost1 = visited[pair.First];
                    var cost2 = visited[pair.Second];
                    var diff = Math.Abs(cost1 - cost2) - 2;

                    if (diff > 0)
                    {
                        cheats.TryAdd(diff, 0);
                        cheats[diff]++;
                    }
                }
            }
        }

        return cheats.Where(x => x.Key >= 100).Sum(x =>x.Value);
    }

    public object Part2()
    {
        var queue = new PriorityQueue<State, int>();
        queue.Enqueue(new State(_start, 20), 1);

        var visited = new Dictionary<State, int>();
        Dictionary<State, int> finishers = [];
        
        while (queue.TryDequeue(out var state, out var cost))
        {
            if (state.Position == _end)
            {
                finishers[state] = cost;
                continue;
            }
            
            
        }
        
        return null!;
    }

    private record State(Vec2 Position, int CheatTimeLeft);
}