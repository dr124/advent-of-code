using Advent.Core;

namespace Advent._2022.Week1;

public class Day4 : IReadInputDay
{
    private Pair[] _input;

    public void ReadData() => _input =
        File.ReadLines("Week1/Day4.txt")
            .Select(line => line
                .Split(',', '-')
                .Select(int.Parse)
                .ToArray())
            .Select(x => new Pair(x[0], x[1], x[2], x[3]))
            .ToArray();

    public object? TaskA() => _input.Count(IsFullyContained);

    public object? TaskB() => _input.Count(IsPartiallyContained);

    private bool IsFullyContained(Pair pair) =>
        (pair.StartA >= pair.StartB && pair.EndA <= pair.EndB)
        || (pair.StartB >= pair.StartA && pair.EndB <= pair.EndA);

    private bool IsPartiallyContained(Pair pair) =>
        pair.StartA <= pair.EndB && pair.EndA >= pair.StartB;
    
    private record Pair(int StartA, int EndA, int StartB, int EndB);
}