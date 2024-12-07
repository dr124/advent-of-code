namespace Advent._2024.Week1;

public class Day4(string[] input) : IDay
{
    private const string Pattern = "XMAS";
    
    public object Part1()
    {
        var query =
            from y in Enumerable.Range(0, input.Length)
            from x in Enumerable.Range(0, input[0].Length)
            where input[y][x] == Pattern[0]
            from direction in Vec2.Adjacent
            where FindPattern((x,y), direction)
            select 1;

        return query.Sum();
    }

    private bool FindPattern(Vec2 xPoint, Vec2 direction)
    {
        // reverse loop = 9% performance boost
        for (var i = Pattern.Length - 1; i >= 1; i--) 
        //for (var i = 1; i < Pattern.Length; i++)
        {
            var pos = xPoint + direction * i;
            if (!pos.IsInBounds(input) || input[pos.Y][pos.X] != Pattern[i])
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
            let chars = (from d in Vec2.Corners select input[y + d.Y][x + d.X]).ToArray()
            where chars.Count(c => c == 'M') == 2
                  && chars.Count(c => c == 'S') == 2
                  && chars[0] != chars[3]
                  && chars[1] != chars[2]
            select chars;

        return query.Count();
    }
} 