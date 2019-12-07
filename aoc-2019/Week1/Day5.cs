using System;
using System.IO;
using System.Linq;

// ReSharper disable EmptyGeneralCatchClause

namespace advent.Week1
{
    public static class Day5
    {
        public static void Execute()
        {
            var ints = File.ReadAllText(@"Week1\input5.txt")
                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();

            var a = new IntcodeComputer(ints) { Input = Enumerable.Repeat(1, 100).ToList() };
            a.Compute();
            Console.WriteLine(string.Join("\n", a.Output));

            var b = new IntcodeComputer(ints) { Input = Enumerable.Repeat(5, 100).ToList() };
            b.Compute();
            Console.WriteLine(string.Join("\n", b.Output));
        }
    }
}