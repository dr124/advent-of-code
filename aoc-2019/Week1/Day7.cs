using System;
using System.IO;
using System.Linq;
// ReSharper disable EmptyGeneralCatchClause

namespace advent.Week1
{
    public static class Day7
    {
        public static int DiagCode = 5;

        public static void Execute()
        {
            //task 1
            var ints = File.ReadAllText(@"Week1\input7.txt")
                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();

            //DiagCode = 1;
            //var a = Compute(ints);

            //Console.WriteLine(a[0]);
            //DiagCode = 5;
            //var b = Compute(ints);

            //Console.WriteLine(b[0]);
        }

        public static int[] Process(int[] ints)
        {
            return null;
        }


     
    }
}