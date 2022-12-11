using Advent.Core;
using Advent.Core.Extensions;

namespace Advent._2022.Week2;

public class Day10 : IReadInputDay
{
    private int[] _input;

    public void ReadData()
    {
        _input = File.ReadAllLines("Week2/Day10.txt")
            .SelectMany(x => x[0] == 'a'
                ? new[] { 0, int.Parse(x[5..]) }
                : new[] { 0 })
            .ToArray();
    }

    public object TaskA()
    {
        var x = 1;
        var strength = 0;

        for (var i = 1; i <= _input.Length; i++)
        {
            if ((i - 20) % 40 == 0)
            {
                strength += i * x;
            }

            Console.Write(x.IsBetween(i % 40 - 2, i % 40) ? '#' : ' ');
            if (i % 40 == 0)
                Console.WriteLine();

            x += _input[i-1];
        }

        return strength;
    }

    public object? TaskB() => null; // done in A, read console
}