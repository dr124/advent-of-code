namespace Advent._2024.Week1;

public class Day4(string[] input) : IDay
{
    private const string Pattern = "XMAS";
    private readonly (int y, int x)[] _straight = [(0, 1), (1, 0), (0, -1), (-1, 0)];
    private readonly (int y, int x)[] _diagonals = [(-1, -1), (1, 1), (-1, 1), (1, -1)];
    
    public object Part1()
    {
        var query =
            from y in Enumerable.Range(0, input.Length)
            from x in Enumerable.Range(0, input[0].Length)
            where input[y][x] == Pattern[0]
            from direction in _straight.Concat(_diagonals)
            where FindPattern((y, x), direction)
            select 1;

        return query.Sum();
    }

    private bool FindPattern((int y, int x) xPoint, (int y, int x) direction)
    {
        // reverse loop = 9% performance boost
        for (var i = Pattern.Length - 1; i >= 1; i--) 
        //for (var i = 1; i < Pattern.Length; i++)
        {
            var pos = (y: xPoint.y + direction.y * i, x: xPoint.x + direction.x * i);
            if (pos.y < 0 || pos.y >= input.Length ||
                pos.x < 0 || pos.x >= input[0].Length ||
                input[pos.y][pos.x] != Pattern[i])
            {
                return false;
            }
        }

        return true;
    }

    public object Part2()
    {
        var query =
            from y in Enumerable.Range(1, input.Length - 2)
            from x in Enumerable.Range(1, input[0].Length - 2)
            where input[y][x] == 'A'
            let chars = (from d in _diagonals select input[y + d.y][x + d.x]).ToArray()
            where chars.Count(c => c == 'M') == 2
                  && chars.Count(c => c == 'S') == 2
                  && chars[0] != chars[1]
                  && chars[2] != chars[3]
            select chars;

        return query.Count();
    }
} 