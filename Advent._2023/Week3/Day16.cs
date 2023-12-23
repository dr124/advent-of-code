using System.Diagnostics;

namespace Advent._2023.Week3;

public class Day16(string[] input) : IDay
{
    private static Vec2 Up = (0, -1);
    private static Vec2 Down = (0, 1);
    private static Vec2 Left = (-1, 0);
    private static Vec2 Right = (1, 0);
    

    [DebuggerDisplay("({X},{Y})")]
    private record Vec2
    {
        public int X;
        public int Y;

        public static implicit operator Vec2((int x, int y) pair) => new() { X = pair.x, Y = pair.y };

        public static Vec2 operator +(Vec2 a, Vec2 b) => (a.X + b.X, a.Y + b.Y);
        public static Vec2 operator -(Vec2 a, Vec2 b) => (a.X - b.X, a.Y - b.Y);
    }

    private Vec2[] Go(char c, Vec2 v)
    {
        return c switch
        {
            '.' => [v],
            ',' => [],
            '/' when v == Up => [Right],
            '/' when v == Down => [Left],
            '/' when v == Left => [Down],
            '/' when v == Right => [Up],
            '\\' when v == Up => [Left],
            '\\' when v == Down => [Right],
            '\\' when v == Left => [Up],
            '\\' when v == Right => [Down],
            '-' when v == Left => [Left],
            '-' when v == Right => [Right],
            '-' when v == Up || v == Down => [Left, Right],
            '|' when v == Up => [Up],
            '|' when v == Down => [Down],
            '|' when v == Left || v == Right => [Up, Down],
            _ => throw new Exception("Unknown dir")
        };
    }
    
    public object Part1() => DoTheThing(Left, Right);

    private int DoTheThing(Vec2 startPosition, Vec2 dir)
    {
        var beams = new Queue<Beam>([new Beam(startPosition, dir)]);
        var visited = new int[input.Length][];
        for (var i = 0; i < input.Length; i++)
        {
            visited[i] = new int[input[i].Length];
        }

        int noNewVisited = 0;
        while (beams.TryDequeue(out var beam))
        {
            //DrawMap(visited);

            var next = beam.Position + beam.Direction;
            var nextChar = At(next);

            if (nextChar != ',')
            {
                if (visited[next.Y][next.X] == 0)
                {
                    noNewVisited = 0;
                }
                else
                {
                    noNewVisited++;
                }
                if (noNewVisited > 100000) // low effort, it's good enough
                {
                    break;
                }
                visited[next.Y][next.X]++;
            }


            var nextDirs = Go(nextChar, beam.Direction);
            foreach (var nextDir in nextDirs)
            {
                beams.Enqueue(new Beam(next, nextDir));
            }
        }

        return visited.Sum(x => x.Count(y => y > 0));
    }
    
    private record Beam(Vec2 Position, Vec2 Direction);
    
    private char At(Vec2 pos)
    {
        if (pos.Y < 0 || pos.X < 0 || pos.Y >= input.Length || pos.X >= input[pos.Y].Length)
            return ',';
        return input[pos.Y][pos.X];
    }
    
    public object Part2()
    {
        var n = input.Length;
        var m = input[0].Length;

        var top = Enumerable.Range(0, m).Select(x => (x, -1)).ToArray();
        var topDir = Down;
        var bottom = Enumerable.Range(0, m).Select(x => (x, n)).ToArray();
        var bottomDir = Up;
        var left = Enumerable.Range(0, n).Select(y => (-1, y)).ToArray();
        var leftDir = Right;
        var right = Enumerable.Range(0, n).Select(y => (m, y)).ToArray();
        var rightDir = Left;

        var starting = top.Select(x => (x, topDir))
            .Concat(bottom.Select(x => (x, bottomDir)))
            .Concat(left.Select(x => (x, leftDir)))
            .Concat(right.Select(x => (x, rightDir)))
            .ToArray();

        var results = starting.AsParallel().Select(x => DoTheThing(x.Item1, x.Item2)).ToArray();

        return results.Max();
    }
}