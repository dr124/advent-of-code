using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Advent._2019.Intcode;
using Advent.Core;

namespace Advent._2019.Week3
{
    public static class Day17
    {
        public static void Execute()
        {
            var ints = File.ReadAllText("Week3/input17.txt")
                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                .Select(long.Parse)
                .ToArray();

            var computer = new Computer(ints);

            var map = new Dictionary<Vec2, int>();
            var pos = new Vec2();
            computer.OnProgramOutput += (s, e) =>
            {
                Console.Write((char) e.OutputValue);
                var o = e.OutputValue;
                if (o == '\n')
                {
                    pos.Y += 1;
                    pos.X = 0;
                    return;
                }

                map[pos] = o == '#' ? 1 : 0;
                pos.X += 1;

            };
            computer.OnInputEmpty += (s, e) =>
            {
                var input = Console.ReadLine();
                computer.Input.EnqueueMany(input.Select(x => (long)x));
                computer.Input.Enqueue('\n');
            };
            computer.Memory[0] = 2;
            computer.Compute(); 
            
            var intt = map.Where(x => IsIntersection(map, x.Key)).ToArray();
            var taskA = intt.Select(x => x.Key.X * x.Key.Y).Sum();
            Console.WriteLine(taskA);

        }

        private static bool IsIntersection(Dictionary<Vec2, int> map, Vec2 pos)
        {
            if (map[pos] != 1)
                return false;

            var adjacent = new [] {(0, 1), (0, -1), (1, 0), (-1, 0)}
                .Select(x => pos + x)
                .ToArray();

            return adjacent.All(x => map.ContainsKey(x) && map[x] == 1);
        }
    }
}
