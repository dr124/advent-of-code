using Advent.Core;

namespace Advent._2022.Week1;

public class Day1 : IReadInputDay
{
    private List<int> _input;

    public void ReadData()
    {
        _input = File.ReadLines("Week1/Day1.txt")
            .Aggregate(new List<int> { 0 }, (list, line) =>
            {
                if (line is "") list.Add(0);
                else list[^1] += int.Parse(line);
                return list;
            });
    }

    public object? TaskA() => _input.Max();

    public object? TaskB() => _input.OrderDescending().Take(3).Sum();
}