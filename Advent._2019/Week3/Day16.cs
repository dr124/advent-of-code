using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using MoreLinq;

namespace Advent._2019.Week3
{
    public static class Day16
    {
        static readonly int[] basePattern = { 0, 1, 0, -1 };
        
        static int Digit(int n) => n >= 0 ? (n % 10) : (-n % 10);

        public static void Execute()
        {
            var ints = "69317163492948606335995924319873"
                //var ints = File.ReadAllBytes("Week3/input16.txt")
                .Select(x => x - 48)
                .ToArray()
                //.Repeat(10000)
                .ToArray();

            var n = ints.Length;

            var phases = 100;
            do
            {
                for (var i = 0; i < n; i++)
                {
                    var sum = 0;
                    for (var j = 0; j < n; j++)
                        sum += basePattern[(j + 1) / (i + 1) % 4]  * ints[j];
                    ints[i] = Digit(sum);
                }
            } while (--phases > 0);

            Console.WriteLine(string.Join("", ints.Take(8)));
        }
    }
}