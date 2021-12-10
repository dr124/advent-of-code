using Advent.Core;
using Advent.Core.Extensions;

namespace Advent._2021.Week1;

internal class Day3 : IReadInputDay<int[][]>
{
    public int[][] Input { get; set; }

    public void ReadData() =>
        Input = File.ReadAllLines("Week1/Day3.txt")
            .Select(x => x.Select(y => y - '0').ToArray())
            .ToArray();

    int MostCommon(int[] tab) => tab.Sum() * 2 >= tab.Length ? 1 : 0;
    int LeastCommon(int[] tab) => tab.Sum() * 2 < tab.Length ? 1 : 0;
    int ToInt(IEnumerable<int> tab) => tab.Select((x, i) => (x == 1 ? 1 : 0) * (1 << i)).Sum();

    public object TaskA()
    {
        var transposed = Input.Transpose();

        var num1 = ToInt(transposed.Select(MostCommon).Reverse());
        var num2 = ToInt(transposed.Select(LeastCommon).Reverse());

        return num1 * num2;
    }

    public object TaskB()
    {
        int Find(Func<int[], int> func, int[][] input)
        {
            for (int i = 0; i < input[0].Length && input.Length > 1; i++)
            {
                var transposed = input.Transpose();
                var common = func(transposed[i]);

                input = input.Where(x => x[i] == common).ToArray();
            }

            return ToInt(input[0].Reverse());
        }

        var num1 = Find(MostCommon, Input);
        var num2 = Find(LeastCommon, Input);

        return num1 * num2;
    }

}