using System.Runtime.CompilerServices;
using Microsoft.Diagnostics.Tracing.Parsers.Clr;

namespace Advent._2023.Week3;

public class Day17(string[] input) : IDay
{
    public object Part1() => DoTheThing(GetNextNodesPart1);
    
    public object Part2() => DoTheThing(GetNextNodesPart2);
    
    private IEnumerable<Node> GetNextNodesPart1(Node n)
    {
        yield return new(n.X + n.Dy, n.Y + n.Dx, n.Dy, n.Dx, 1);
        yield return new(n.X - n.Dy, n.Y - n.Dx, -n.Dy, -n.Dx, 1);

        if (n.Streak < 3)
        {
            yield return new(n.X + n.Dx, n.Y + n.Dy, n.Dx, n.Dy, n.Streak + 1);
        }
    }

    private IEnumerable<Node> GetNextNodesPart2(Node n)
    {
        if (n.Streak >= 4)
        {
            yield return new(n.X + n.Dy, n.Y + n.Dx, n.Dy, n.Dx, 1, false);
            yield return new(n.X - n.Dy, n.Y - n.Dx, -n.Dy, -n.Dx, 1, false);
        }
        
        if (n.Streak < 10)
        {
            yield return new(n.X + n.Dx, n.Y + n.Dy, n.Dx, n.Dy, n.Streak + 1, n.Streak >= 3);
        }
    }

    delegate IEnumerable<Node> NextNodes(Node n);
    
    private int DoTheThing(NextNodes getNextNodes)
    {
        var start = (x: 0, y: 0);
        var end = (x: input[0].Length - 1, y: input.Length - 1);

        Dictionary<Node, int> visited = [];
        PriorityQueue<Node, int> queue = new();

        var startNode = new Node(start.x, start.y, 1, 0, 0);
        visited[startNode] = 0;
        queue.Enqueue(startNode, 0);


        while (queue.TryDequeue(out var node, out _))
        {
            var cost = visited[node];
            if ((node.X, node.Y) == end && node.CanFinish)
            {
                return cost;
            }

            foreach (var newNode in getNextNodes(node))
            {
                if (!IsInMap(newNode.X, newNode.Y))
                {
                    continue;
                }

                var newCost = visited[node] + Cost(newNode.X, newNode.Y);
                if (visited.TryGetValue(newNode, out var oldCost) && oldCost <= newCost)
                {
                    continue;
                }

                visited[newNode] = newCost;
                queue.Enqueue(newNode, newCost);
            }
        }

        return -1;
    }
    
    private record Node(int X, int Y, int Dx, int Dy, int Streak, bool CanFinish = true);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private int Cost(int x, int y) => input[y][x] - '0';

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool IsInMap(int x, int y) => y >= 0 && x >= 0 && y < input.Length && x < input[y].Length;
}