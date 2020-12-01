using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Advent._2019.Intcode;
using Advent.Core;

namespace Advent._2019.Week3
{
    public static class Day15
    {
        private static int taskA;
        private static int taskB;
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

            var forks = new Stack<Vec2>(); //    ----<=

            computer.OnInputEmpty += (s, e) =>
            {
                do
                {
                    var dirs = new Vec2[]
                    {
                        (0, -1),
                        (0, 1),
                        (-1, 0),
                        (1, 0)
                    };
                    var canGo = dirs
                        .Where(x => !map.ContainsKey(pos + x))
                        .ToArray();

                    dir = canGo.Length > 0
                        ? canGo[0]
                        : forks.Count > 0
                            ? forks.Pop() - pos // last pos
                            : -1; // spread air;

                } while (dir == 0);

                if (dir == -1)
                {
                    SpreadAir(map);
                    return;
                }

                computer.Input.Enqueue(dir.DirToCommand());
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
                        forks.Push(pos);
                        break;
                    case 2:
                        pos += dir;
                        map[pos] = 2;
                        forks.Push(pos);
                        taskA = forks.Count;
                        break;
                }

                DrawMap(map);
            };
            computer.Compute();
        }

        private static int DirToCommand(this Vec2 dir)
        {
            return dir switch
            {
                { X: 0, Y: -1 } => 1,
                { X: 0, Y: 1 } => 2,
                { X: -1, Y: 0 } => 3,
                { X: 1, Y: 0 } => 4
            };
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
                        p + (1, 0),
                        p + (-1, 0),
                        p + (0, -1),
                        p + (0, 1)
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
            Vec2 offset = (-21, -21);
            Console.SetCursorPosition(0, 0);
            for (var y = 0; y < 41; y++)
            {
                for (var x = 0; x < 41; x++)
                {
                    var poss = (x, y) + offset;
                    var str = "░░";
                    if (poss == pos)
                        str = "DD";
                    else if (map.ContainsKey(poss))
                        str = map[poss] switch
                        {
                            0 => "██",
                            1 => "  ",
                            2 => "@@",
                        };
                    Console.Write(str);
                }

                Console.WriteLine();
            }

            Console.WriteLine($"taskA: {taskA}");
            Console.WriteLine($"taskB: {taskB}");
        }
    }
}