using Advent.Core;
using BenchmarkDotNet.Attributes;

namespace Advent._2021.Week1;

internal class Day7 : IReadInputDay
{
    private int[] Input;
    private int Min;
    private int Max;

    public void ReadData()
    {
        Input = File.ReadAllText("Week1/Day7.txt")
            .Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(int.Parse)
            .ToArray();
        Min = Input.Min();
        Max = Input.Max();
    }

    public object TaskA() => FindSmallestFuel(Math.Abs);
    public object TaskB() => FindSmallestFuel(x => Sum1ToN(Math.Abs(x)));

    private int Sum1ToN(int x) => x * (x + 1) / 2;

    private int FindSmallestFuel(Func<int, int> costFunc) =>
        Enumerable.Range(Min, Max - Min)
            .Select(target => Input.Select(position => costFunc(position - target)).Sum())
            .Min();
}