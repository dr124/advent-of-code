using System.Text;

namespace Advent._2024.Week3;

public class Day16(string[] input) : IDay
{
    private readonly Vec2 _start = Extensions.ReadInput(input, 'S').First();
    private int _bestPathLength;
    
    public object Part1()
    {
        var queue = new PriorityQueue<Step, CostPath>();
        var costs = new Dictionary<Step, int>();

        var startQ = new Step(_start, Vec2.Right);
        queue.Enqueue(startQ, new CostPath(0, [_start]));
        costs[startQ] = 0;

        var winnerPath2 = new List<Vec2>();
        var bestPathCost2 = int.MaxValue;
        while (queue.TryDequeue(out var q, out var cp))
        {
            if (q.Position.On(input) == 'E')
            {
                if (cp.Cost < bestPathCost2)
                {
                    bestPathCost2 = cp.Cost;
                    winnerPath2.Clear();
                }

                if (cp.Cost == bestPathCost2)
                {
                    winnerPath2.AddRange(cp.Path);
                }
            }

            if (costs.TryGetValue(q, out var previousCost) && cp.Cost > previousCost)
            {
                continue; // nothing to do, this path is worse
            }

            TryEnqueue(new (q.Position + q.Direction, q.Direction), cp.Cost + 1, cp.Path, true);
            TryEnqueue(new (q.Position, q.Direction.Rotate(Rotation.Clockwise)), cp.Cost + 1000, cp.Path);
            TryEnqueue(new (q.Position, q.Direction.Rotate(Rotation.CounterClockwise)), cp.Cost + 1000, cp.Path);
        }

        _bestPathLength = winnerPath2.Distinct().Count();
        return bestPathCost2;

        void TryEnqueue(Step nextQ, int newCost, List<Vec2> winnerPath, bool append = false)
        {
            if (nextQ.Position.On(input) != '#' && (!costs.ContainsKey(nextQ) || costs[nextQ] >= newCost))
            {
                queue.Enqueue(nextQ, new CostPath(newCost, append ? winnerPath.Append(nextQ.Position).ToList() : winnerPath));
                costs[nextQ] = newCost;
            }
        }
    }

    public object Part2() => _bestPathLength;

    private readonly record struct Step(Vec2 Position, Vec2 Direction);
    
    private readonly record struct CostPath(int Cost, List<Vec2> Path) : IComparable<CostPath>
    {
        // required by PriorityQueue
        public int CompareTo(CostPath other) => Cost.CompareTo(other.Cost);
    }
}