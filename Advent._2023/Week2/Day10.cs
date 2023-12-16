using System.Diagnostics;

namespace Advent._2023.Week2;

public class Day10(string[] input) : IDay
{
    private static Vec2 U = (0, -1);
    private static Vec2 D = (0, 1);
    private static Vec2 L = (-1, 0);
    private static Vec2 R = (1, 0);

    private readonly List<Vec2> _loop = [];
    private readonly Vec2[] _adjacent = [U, R, L, D];
    private readonly (Vec2 direction, char[] validChars)[] _directions =
    [
        (U, ['F', '7', '|']),
        (D, ['J', 'L', '|']),
        (L, ['L', 'F', '-']),
        (R, ['7', 'J', '-'])
    ];

    public object Part1()
    {
        Vec2 startPos = (0, 0);
        for (int y = 0; y < input.Length; y++)
            for (int x = 0; x < input[y].Length; x++)
                if (At((x, y)) == 'S')
                    startPos = (x, y);

        _loop.Add(startPos);

        var current = GetSecondPipe(startPos);
        while (current != startPos)
        {
            _loop.Add(current);
            
            var prev = _loop[^2];
            var pipe = At(current);

            current = GetConnections(pipe)
                .Select(c => current + c)
                .First(c => c != prev && At(c) != '.');
        }

        return _loop.Count / 2;
    }

    public object Part2()
    {
        var loopHash = _loop.ToHashSet();
        var filled = new HashSet<Vec2>();

        _loop.Reverse(); // Fill from the inside

        // Fill the insides of the loop
        for (var i = 0; i < _loop.Count; i++)
        {
            var next = _loop[(i + 1) % _loop.Count];
            var now = _loop[i];
            var prev = _loop[(i - 1 + _loop.Count) % _loop.Count];

            var diffPrev = GetDirection(now, prev);
            var diffNext = GetDirection(next, now);
            var dir = diffPrev + diffNext;

            var insides = GetInsides(dir);
            foreach (var point in insides)
            {
                var pos = now + point;
                if (!loopHash.Contains(pos))
                {
                    filled.Add(pos);
                }
            }
        }

        // Flood 
        var toFill = new Stack<Vec2>(filled);
        while (toFill.TryPop(out var pos))
        {
            foreach (var v in _adjacent)
            {
                var next = pos + v;
                if (filled.Contains(next))
                    continue;

                if (loopHash.Contains(next))
                    continue;

                var c = At(next);
                if (c == ',')
                    continue;

                filled.Add(next);
                toFill.Push(next);
            }
        }

        return filled.Count;
    }

    private string GetDirection(Vec2 v1, Vec2 v2)
    {
        var diff = v1 - v2;
        if (diff == U)
            return "U";
        if (diff == D)
            return "D";
        if (diff == L)
            return "L";
        if (diff == R)
            return "R";

        throw new Exception("Unknown dir");
    }
    
    private Vec2 GetSecondPipe(Vec2 startPos) => _directions
        .Select(d => new { Position = startPos + d.direction, Chars = d.validChars })
        .First(p => p.Chars.Contains(At(p.Position)))
        .Position;

    private static Vec2[] GetConnections(char pipe) => pipe switch
    {
        '-' => [L, R],
        '|' => [U, D],
        'L' => [U, R],
        'J' => [U, L],
        '7' => [D, L],
        'F' => [D, R],
        '.' or ',' or 'S' => [],
        _ => throw new ArgumentException($"Unknown pipe: {pipe}")
    };

    private static Vec2[] GetInsides(string dir) => dir switch
    {
        "RR" => [U],
        "LL" => [D],
        "UU" => [L],
        "DD" => [R],
        "RD" => [U, R],
        "DL" => [R, D],
        "LU" => [D, L],
        "UR" => [L, U],
        _ => []
    };

    private char At(Vec2 pos)
    {
        if (pos.Y < 0 || pos.X < 0 || pos.Y >= input.Length || pos.X >= input[pos.Y].Length)
            return ',';
        return input[pos.Y][pos.X];
    }
    
    [DebuggerDisplay("({X},{Y})")]
    private record Vec2
    {
        public int X;
        public int Y;

        public static implicit operator Vec2((int x, int y) pair) => new() { X = pair.x, Y = pair.y };

        public static Vec2 operator +(Vec2 a, Vec2 b) => (a.X + b.X, a.Y + b.Y);
        public static Vec2 operator -(Vec2 a, Vec2 b) => (a.X - b.X, a.Y - b.Y);
    }
}