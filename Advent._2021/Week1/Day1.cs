using Advent.Core;

namespace Advent._2021.Week1;

internal class Day1 : IReadInputDay<int[]>
{
    public int[] Input { get; set; }
    public void ReadData() =>
        Input = File.ReadAllLines("Week1/Day1.txt")
            .Select(int.Parse)
            .ToArray();

    public object TaskA()
    {
        var result = 0;
        for (var i = 1; i < Input.Length; i++)
            if (Input[i] > Input[i - 1])
                result++;

        return result; // 1502
    }

    public object TaskB()
    {
        var result = 0;
        for (var i = 3; i < Input.Length; i++)
            if (Input[i] > Input[i - 3])
                result++;

        return result; // 1538
    }
}
