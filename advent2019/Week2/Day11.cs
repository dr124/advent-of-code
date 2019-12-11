using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using advent2019.Intcode;

#pragma warning disable 8509

namespace advent2019.Week2
{
    public static class Day11
    {
        public static void Execute()
        {
            var ints = File.ReadAllText(@"Week2\input11.txt")
                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                .Select(long.Parse)
                .ToArray();

            var a = Process(ints);
            Console.Write($"A: {a}, B: CGPJCGCL");
        }

        public static int Process(long[] ints)
        {
            var map = new Dictionary<Point, int>();
            var computer = new Computer(ints);

            var output = 0;
            var rotations = new[] {(0, 1), (1, 0), (0, -1), (-1, 0)};
            var rot = 0;
            Point pos = 0;

            computer.Input.Enqueue(1);
            computer.OnProgramOutput += (s, e) =>
            {
                switch (output)
                {
                    case 0:
                        output = 1;
                        return;
                    case 1:
                        output = 0;
                        map[pos] = (int) computer.Output[^2];
                        rot = (rot + ((int) computer.Output[^1] * 2 - 1) + rotations.Length) % rotations.Length;
                        pos += rotations[rot];
                        computer.Input.Enqueue(map.ContainsKey(pos) ? map[pos] : 0);
                        return;
                }
            };

            computer.Compute();

            for (var y = 0; y >= -6; y--, Console.WriteLine()) // no braces!
            for (var x = 0; x < 40; x++)
                Console.Write(map.ContainsKey((x, y)) && map[(x, y)] == 1 ? "X" : " ");

            return map.Count;
        }
    }
}