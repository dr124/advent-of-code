using System;
using System.IO;
using System.Linq;
using Advent._2019.Intcode;

// ReSharper disable EmptyGeneralCatchClause

namespace Advent._2019.Week1;

public static class Day5
{
    public static void Execute()
    {
        var ints = File.ReadAllText(@"Week1\input5.txt")
            .Split(",", StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToArray();

        var a = new Computer(ints);
        a.Input.Enqueue(1);
        a.Compute();
        Console.WriteLine(string.Join("\n", a.Output));

        var b = new Computer(ints);
        b.Input.Enqueue(5);
        b.Compute();
        Console.WriteLine(string.Join("\n", b.Output));
    }
}