namespace Advent._2024.Week2;

public class Day13(string[] input) : IDay
{
    private readonly Prize[] _prizes = ReadInput(input);
    public object Part1() => _prizes.Select(CalcTokens).Sum();

    public object Part2() => _prizes
        .Select(p => p with { Px = p.Px + 10_000_000_000_000L, Py = p.Py + 10_000_000_000_000L })
        .Select(CalcTokens)
        .Sum();

    private long CalcTokens(Prize prize)
    {
        var det = prize.Ax * prize.By - prize.Bx * prize.Ay;
        var detA = prize.Px * prize.By - prize.Bx * prize.Py;
        var detB = prize.Ax * prize.Py - prize.Px * prize.Ay;

        var a = detA / det;
        var b = detB / det;

        if (a * det != detA || b * det != detB)
        {
            return 0;
        }
        
        return a * 3 + b * 1;
    }
    
    private static Prize[] ReadInput(string[] input) => input.Chunk(4).Select(ReadPrize).ToArray();

    private static Prize ReadPrize(string[] input)
    {
        var a = input[0].Split(['+', ','], StringSplitOptions.TrimEntries);
        var b = input[1].Split(['+', ','], StringSplitOptions.TrimEntries);
        var p = input[2].Split(['=', ','], StringSplitOptions.TrimEntries);

        return new Prize(
            int.Parse(a[1]), int.Parse(a[3]),
            int.Parse(b[1]), int.Parse(b[3]),
            int.Parse(p[1]), int.Parse(p[3]));
    }

    private record struct Prize(long Ax, long Ay, long Bx, long By, long Px, long Py);
}