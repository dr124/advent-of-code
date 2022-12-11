using System.Text;
using System.Xml.Linq;
using Advent.Core;
using Advent.Core.Extensions;

namespace Advent._2022.Week2;

public class Day10 : IReadInputDay
{
    private int?[] _input;

    public void ReadData()
    {
        _input = File.ReadAllLines("Week2/Day10.txt")
            .Select(x => x[0] == 'a' ? int.Parse(x[4..]) : (int?)null)
            .ToArray();
    }

    public object TaskA()
    {
        var cycleStrength = new List<int> { 1 };
        int register = 1;

        foreach (var instr in _input)
        {
            if (instr is not null)
            {
                cycleStrength.Add(register);
            }

            cycleStrength.Add(register);
            register += instr ?? 0;
        }

        for (int y = 0; y < 6; y++)
        {
            for (int x = 0; x < 40; x++)
            {
                Console.Write(cycleStrength[y * 40 + x].IsBetween(x - 2, x) ? '#' : '.');
            }
            Console.WriteLine();
        }

        return new[] { 20, 60, 100, 140, 180, 220 }
            .Select(x => cycleStrength[x] * x)
            .Sum();
    }

    public object? TaskB() => null; // done in A, read console
}