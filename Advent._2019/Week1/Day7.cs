using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Advent._2019.Intcode;
using Advent.Core;
using MoreLinq;

// ReSharper disable EmptyGeneralCatchClause

namespace Advent._2019.Week1
{
    public static class Day7
    {
        public static void Execute()
        {
            var ints = File.ReadAllText(@"Week1\input7.txt")
                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();

            var taskA = (..5).To().Permutations()
                .Select(seq => ProcessA(ints, seq.ToArray()))
                .Max();

            var taskB = (5..10).To().Permutations()
                .Select(seq => ProcessB(ints, seq.ToArray()))
                .Max();

            Console.WriteLine($"A: {taskA}, B: {taskB}");
        }

        public static long ProcessA(int[] ints, int[] seq)
        {
            var computers = seq.Select(x => new Computer(ints)).ToArray();
            computers.ForEach((x, i) => x.Input.Enqueue(seq[i]));
            computers[0].Input.Enqueue(0); // start value

            computers.ForEach((x, i) =>
            {
                x.Compute();
                computers[(i + 1) % seq.Length].Input.Enqueue(x.Output[^1]);
            });

            return computers[^1].Output[^1];
        }

        public static long ProcessB(int[] ints, int[] seq)
        {
            var computers = seq.Select(x => new Computer(ints)).ToArray();
            computers.ForEach((x, i) => x.Input.Enqueue(seq[i]));
            computers[0].Input.Enqueue(0); // start value

            computers.ForEach((x, i) => x.OnProgramOutput += (s, args) =>
                computers[(i + 1) % seq.Length].Input.Enqueue(args.OutputValue));

            long output = 0;
            computers[^1].OnProgramFinish += (sender, args) => output = computers[^1].Output.Max();

            Task.WaitAll(computers.Select(x => Task.Run(x.Compute)).ToArray());

            return output;
        }
    }
}