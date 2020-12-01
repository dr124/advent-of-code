using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Advent._2019.Week1
{
    public static class Day3
    {
        public static void Execute()
        {
            var moves = File.ReadAllLines(@"Week1\input3.txt");
            var result = Process(moves);

            Console.WriteLine($"A: {result.Item1}, B: {result.Item2}");
        }

        public static (int, int) Process(string[] input)
        {
            var wire1 = ProcessWire(input[0]);
            var wire2 = ProcessWire(input[1]);

            var closestIntersection = wire1.Keys.Intersect(wire2.Keys)
                .Min(x => Math.Abs(x.Item1) + Math.Abs(x.Item2));

            var firstIntersection = wire1.Keys.Intersect(wire2.Keys)
                .Min(x => wire1[x] + wire2[x]);

            return (closestIntersection, firstIntersection);
        }

        public static Dictionary<(int, int), int> ProcessWire(string moves)
        {
            var dict = new Dictionary<(int, int), int>();
            int x = 0, y = 0, step = 0;

            foreach (var move in moves.Split(","))
            {
                var dist = int.Parse(move.Substring(1));
                for (var i = 0; i < dist; i++)
                    if (move[0] == 'U')
                        dict.TryAdd((x, ++y), ++step);
                    else if (move[0] == 'D')
                        dict.TryAdd((x, --y), ++step);
                    else if (move[0] == 'L')
                        dict.TryAdd((--x, y), ++step);
                    else if (move[0] == 'R')
                        dict.TryAdd((++x, y), ++step);
            }

            return dict;
        }
    }
}