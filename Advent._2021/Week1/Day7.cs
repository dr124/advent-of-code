using Advent.Core;

namespace Advent._2021.Week1;

internal class Day7 : IReadInputDay
{
    private int[] Input;

    public void ReadData() =>
        Input = File.ReadAllText("Week1/Day7.txt")
            .Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(int.Parse)
            .OrderBy(x=>x)
            .ToArray();

    public object TaskA() => CalculateFuelCost(Median(Input), Math.Abs);
    public object TaskB() => CalculateFuelCost((int)Input.Average(), x => Sum1ToN(Math.Abs(x)));

    private int Sum1ToN(int x) => x * (x + 1) / 2;
    private int Median(int[] x) => x.Length % 2 == 0 ? (x[x.Length / 2] + x[x.Length / 2 - 1]) / 2 : x[x.Length / 2];
    private int CalculateFuelCost(int target, Func<int, int> cost) => Input.Select(position => cost(position - target)).Sum();
}