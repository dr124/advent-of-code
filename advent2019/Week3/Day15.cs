using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using advent2019.Intcode;

namespace advent2019.Week3
{
    public static class Day15
    {
        private static int taskA;
        private static int taskB;
        private static Vec2 offset = (-45, -30);
        private static Vec2 pos = 0;

        public static void Execute()
        {
            var ints = File.ReadAllText(@"Week3\input15.txt")
                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                .Select(long.Parse)
                .ToArray();

            var map = new Dictionary<Vec2, int>();
            var computer = new Computer(ints);
            var dir = (0, 0).Vec2();
            var command = 0;
            computer.OnInputEmpty += (s, e) =>
            {
                do
                {
                    var k = Console.ReadKey();
                    (dir, command) = k.Key switch
                    {
                        ConsoleKey.UpArrow => ((0, -1), 1),
                        ConsoleKey.DownArrow => ((0, 1), 2),
                        ConsoleKey.LeftArrow => ((-1, 0), 3),
                        ConsoleKey.RightArrow => ((1, 0), 4),
                        _ => ((0, 0), 0)
                    };

                    if (k.Key == ConsoleKey.X)
                        SpreadAir(map);

                } while (command == 0);

                computer.Input.Enqueue(command);
            };
            computer.OnProgramOutput += (s, e) =>
            {
                switch (e.OutputValue)
                {
                    case 0:
                        map[pos + dir] = 0;
                        break;
                    case 1:
                        pos += dir;
                        map[pos] = 1;
                        taskA++;
                        break;
                    case 2:
                        pos += dir;
                        map[pos] = 2;
                        break;
                }
                DrawMap(map);
            };
            computer.Compute();
        }

        private static void SpreadAir(Dictionary<Vec2, int> map)
        {
            while (true)
            {
                var xd = map.Where(x => x.Value == 2).ToList();
                if (map.Count(x => x.Value == 1) == 0)
                    break;
                foreach (var (p, v) in xd)
                {
                    var pp = new[]
                    {
                        p + (0, 1),
                        p + (0, -1),
                        p + (1, 0),
                        p + (-1, 0)
                    };
                    foreach (var ppp in pp)
                        if (map.ContainsKey(ppp) && map[ppp] == 1)
                            map[ppp] = 2;
                }
                taskB++;
                DrawMap(map);
            }
        }

        private static void DrawMap(Dictionary<Vec2, int> map)
        {
            Console.SetCursorPosition(0, 0);
            for (var y = 0; y < 60; y++)
            {
                for (var x = 0; x < 80; x++)
                {
                    var poss = (x, y) + offset;
                    var str = "  ";
                    if (map.ContainsKey(poss))
                        str = map[poss] switch
                        {
                            0 => "██",
                            1 => "░░",
                            2 => "@@",
                            _ => "  "
                        };
                    if (poss == pos)
                        str = "DD";
                    Console.Write(str);
                }
                Console.WriteLine();
            }
            Console.WriteLine($"taskA: {taskA}");
            Console.WriteLine($"taskB: {taskB}");
        }
    }
}