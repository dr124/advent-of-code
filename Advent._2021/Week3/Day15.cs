using Advent.Core;
using Advent.Core.Extensions;

namespace Advent._2021.Week3;

internal class Day15 : IReadInputDay
{
    public int[,] Input { get; set; }

    public void ReadData() =>
        Input = File.ReadAllLines("Week3/Day15.txt")
            .Select(x => x.Select(c => c - '0').ToArray())
            .ToArray()
            .ToMatrix();

    public object TaskA() => XD(Input);
    public object TaskB() => XD(EnlargeMap(Input, 5));

    private int[,] EnlargeMap(int[,] input, int factor)
    {
        var m = input.GetLength(0);
        var n = input.GetLength(0);

        var map = new int[m * factor, n * factor];
        
        var M = map.GetLength(0);
        var N = map.GetLength(1);

        for (var i = 0; i < M; i++)
        for (int j = 0; j < N; j++)
            map[i, j] = (input[i % m, j % n] - 1 + (i / m) + (j / n)) % 9 + 1;
        return map;
    }

    private int XD(int[,] map)
    {
        var M = map.GetLength(0);
        var N = map.GetLength(1);

        var costs = new int[M, N];

        var queue = new PriorityQueue<(int x, int y), int>();
        queue.Enqueue((0, 0), 0);

        void TryEnqueue(int x, int y, int currentCost)
        {
            if (x >= M || x < 0 || y >= N || y < 0) return;
            if (costs[x, y] > 0 && currentCost + map[x, y] > costs[x, y]) return;

            queue.Enqueue((x,y), currentCost);
        }
        
        while (queue.TryDequeue(out var current, out int totalCost))
        {
            var x = current.x;
            var y = current.y; 
            var newCost = totalCost + map[x, y];

            if (costs[x, y] > 0) 
                continue;

            costs[x,y] = newCost;
            TryEnqueue(x + 1, y, newCost);
            TryEnqueue(x - 1, y, newCost);
            TryEnqueue(x, y + 1, newCost);
            TryEnqueue(x, y - 1, newCost);
        }

        return costs[M - 1, N - 1] - costs[0, 0];
    }
}