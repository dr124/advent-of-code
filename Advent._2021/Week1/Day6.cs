using Advent.Core;

namespace Advent._2021.Week1;

internal class Day6 : IReadInputDay
{
    public long[] Input { get; set; }

    public void ReadData()
    {
        var times = File.ReadAllText("Week1/Day6.txt")
            .Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(int.Parse);

        Input = new long[9];
        foreach (var x in times)
            Input[x] += 1;
    }

    public object TaskA() => LiveForNDays(Input.ToArray(), 80);

    public object TaskB() => LiveForNDays(Input.ToArray(), 256);

    long LiveForNDays(long[] fish, int days)
    {
        for (var day = 0; day < days; day++)
        {
            var @new = fish[0];
            for (var i = 1; i < fish.Length; i++)
                fish[i - 1] = fish[i];
            fish[6] += fish[8] = @new; 
        }

        return fish.Sum();
    }
}