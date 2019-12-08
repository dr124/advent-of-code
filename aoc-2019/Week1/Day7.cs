using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MoreLinq;

// ReSharper disable EmptyGeneralCatchClause

namespace advent.Week1
{
    public static class Day7
    {
        //find the largest output signal that can be sent to the thrusters 
        public static void Execute()
        {
            var ints = File.ReadAllText(@"Week1\input7.txt")
                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();

            var taskA = Permutations(new[] {0, 1, 2, 3, 4})
                .AsParallel()
                .Select(seq => Process(ints, seq.ToArray()))
                .OrderByDescending(x => x[4])
                .ToList();

           

        }

        static IEnumerable<IEnumerable<int>> Permutations(this IEnumerable<int> values)
        {
            return values.Count() == 1 ?
                new[] { values } 
                : values.SelectMany(v => 
                    Permutations(values.Where(x => x != v)), (v, p) => Enumerable.Prepend(p, v));
        }

        public static int[] Process(int[] ints, int[] seq)
        {

            var c1 = new IntcodeComputer(ints) { Input = { seq[0], 0} };
            var c2 = new IntcodeComputer(ints) { Input = { seq[1] } };
            var c3 = new IntcodeComputer(ints) { Input = { seq[2] } };
            var c4 = new IntcodeComputer(ints) { Input = { seq[3] } };
            var c5 = new IntcodeComputer(ints) { Input = { seq[4] } };


            c1.Compute(); c2.Input.Add(c1.Output.Last());
            c2.Compute(); c3.Input.Add(c2.Output.Last());
            c3.Compute(); c4.Input.Add(c3.Output.Last());
            c4.Compute(); c5.Input.Add(c4.Output.Last());
            c5.Compute(); 

            return new[]
            {
                c1.Output.Last(),
                c2.Output.Last(),
                c3.Output.Last(),
                c4.Output.Last(),
                c5.Output.Last()
            };

        }



    }
}