using System;
using System.IO;
using System.Linq;

namespace advent2019.Week2
{
    public static class Day9
    {
        public static void Execute()
        {
            var ints = File.ReadAllText(@"Week2\input9.txt")
                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();

            var (a,b) = Process(ints);
            Console.Write($"A: {a}, B: {b}");
        }

        public static (long,long) Process(int[] ints)
        {
            var computerA = new Intcode.Computer(ints, "A");
            computerA.Input.Enqueue(1);
            computerA.Compute();

            var computerB = new Intcode.Computer(ints, "B");
            computerB.Input.Enqueue(2);
            computerB.OnProgramOutput += (s, e) => computerB.Stop = true;
            computerB.Compute();

            return (computerA.Output[^1], computerB.Output[^1]);
        }
    }
}