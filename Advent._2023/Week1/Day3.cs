using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Advent._2023.Week1;

public class Day3(string[] input) : IDay
{
    private readonly Vec2[] _directions = [(1, 0), (-1, 0), (0, 1), (0, -1), (-1, 1), (1, -1), (1, 1), (-1, -1)];
    private readonly HashSet<Vec2> _visited = [];

    public object Part1()
    {
        var sum = 0;
        for (var y = 0; y < input.Length; y++)
        for (var x = 0; x < input[y].Length; x++)
        {
            var c = At((x, y));
            if (!char.IsDigit(c) && c != '.')
            {
                sum += FindAdjacentNumbers((x, y));
            }
        }

        return sum;
    }

    public object Part2()
    {
        _visited.Clear();
        
        var sum = 0;
        for (var y = 0; y < input.Length; y++)
        for (var x = 0; x < input[y].Length; x++)
        {
            if (At((x, y)) is '*')
            {
                sum += FindExactlyTwoAdjacentNumbers((x, y));
            }
        }

        return sum;
    }

    private int FindAdjacentNumbers(Vec2 v) => _directions
        .Select(dir => FindNumber(v + dir))
        .Where(x => x != null)
        .Sum(int.Parse!);

    private int FindExactlyTwoAdjacentNumbers(Vec2 v)
    {
        var numbers = _directions
            .Select(dir => FindNumber(v + dir))
            .Where(x => x != null)
            .Select(int.Parse!)
            .ToList();

        return numbers.Count == 2
            ? numbers.Aggregate(1, (a, b) => a * b) 
            : 0;
    }

    private string? FindNumber(Vec2 pos)
    {
        if (pos.X < 0
            || pos.Y < 0 
            || pos.X >= input[0].Length 
            || pos.Y >= input.Length
            || _visited.Contains(pos) 
            || !char.IsDigit(At(pos)))
        {
            return null;
        }

        _visited.Add(pos);
        var here = At(pos);
        var left = FindNumber(pos + (-1, 0));
        var right = FindNumber(pos + (1, 0));
        return left + here + right;
    }

    private char At(Vec2 pos) => input[pos.Y][pos.X];

    [DebuggerDisplay("({X},{Y})")]
    private struct Vec2
    {
        public int X;
        public int Y;

        public static implicit operator Vec2((int x, int y) pair) => new() { X = pair.x, Y = pair.y };
        
        public static Vec2 operator +(Vec2 a, Vec2 b) => (a.X + b.X, a.Y + b.Y);
    }
}