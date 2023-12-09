using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Advent._2023.Week2;

public class Day9(string[] input) : IDay
{
    private History[] _histories = input.Select(History.ParseLine).ToArray();

    public object Part1() => _histories.Sum(ExtrapolateForward);

    public object Part2() => _histories.Sum(ExtrapolateBackward);

    private int ExtrapolateForward(History history)
    {
        var d = 0;
        for (var i = history.Data.Count - 2; i >= 0; i--)
        {
            d += history.Data[i][^1];
        }

        return d;
    }

    private int ExtrapolateBackward(History history)
    {
        var d = 0;
        for (var i = history.Data.Count - 2; i >= 0; i--)
        {
            d = -d + history.Data[i][0];
        }

        return d;
    }

    private record History
    {
        public List<int[]> Data { get; }
        
        private History(int[] sequence)
        {
            Data = [sequence];

            while (sequence.Any(x => x != 0))
            {
                sequence = Derivative(sequence);
                Data.Add(sequence);
            }
        }

        private static int[] Derivative(int[] history)
        {
            var derivative = new int[history.Length - 1];
            for (var i = 0; i < derivative.Length; i++)
            {
                derivative[i] = history[i + 1] - history[i];
            }

            return derivative;
        }

        public static History ParseLine(string line)
        {
            return new History(line.Split(' ').Select(int.Parse).ToArray());
        }
    }
}