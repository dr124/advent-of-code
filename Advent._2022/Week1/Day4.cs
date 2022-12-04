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
            .Select(x => new Pair(new(x[0], x[1]), new(x[2], x[3])))
            .ToArray();

    public object? TaskA() => _input.Count(IsFullyContained);

    public object? TaskB() => _input.Count(IsPartiallyContained);

    private bool IsFullyContained(Pair pair) =>
        (pair.A.Start >= pair.B.Start && pair.A.End <= pair.B.End)
        || (pair.B.Start >= pair.A.Start && pair.B.End <= pair.A.End);

    private bool IsPartiallyContained(Pair pair) =>
        pair.A.Start <= pair.B.End && pair.A.End >= pair.B.Start;
    
    private record Section(int Start, int End);

    private record Pair(Section A, Section B);
}