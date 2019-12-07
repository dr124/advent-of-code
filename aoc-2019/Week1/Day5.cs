using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
// ReSharper disable EmptyGeneralCatchClause

namespace advent.Week1
{
    public static class Day5
    {
        public static void Execute()
        {
            //task 1
            var ints = File.ReadAllText(@"Week1\input5.txt")
                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();

            var a = new IntcodeComputer(ints) { DiagCode = 1 };
            a.Process();

            var b = new IntcodeComputer(ints) { DiagCode = 5 };
            b.Process();
        }
    }
}