using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Advent._2019.Intcode;
using Advent.Core_2019_2020;

#pragma warning disable 8509

namespace Advent._2019.Week2;

public static class Day13
{
    public static void Execute()
    {
        var ints = File.ReadAllText(@"Week2\input13.txt")
            .Split(",", StringSplitOptions.RemoveEmptyEntries)
            .Select(long.Parse)
            .ToArray();

        var (a, b) = Process(ints);
        Console.Write($"A: {a}, B: {b}");
    }

    public static (int, int) Process(long[] ints)
    {
        var taskA = -1;
        var taskB = -1;

        var map = new Dictionary<Vec2, int>();
        var ballX = 0;
        var paddleX = 0;
        var state = 0;

        var computer = new Computer(ints);
        computer.Memory[0] = 2;
        computer.OnProgramOutput += (s, e) =>
        {
            if (++state % 3 != 0)
                return;

            var x = (int) computer.Output[^3];
            var y = (int) computer.Output[^2];
            var t = (int) computer.Output[^1];
            if (x == -1 && y == 0)
            {
                taskB = t;
                return;
            }

            map[(x, y)] = t;
            if (t == 4) ballX = x;
            if (t == 3) paddleX = x;
        };
        computer.OnInputEmpty += (s, e) =>
        {
            if (taskA == -1)
                taskA = map.Count(x => x.Value == 2);
            computer.Input.Enqueue(ballX.CompareTo(paddleX));
        };
        computer.Compute();

        return (taskA, taskB);
    }

    //for fun
    private static void DrawMap(Dictionary<Vec2, int> map)
    {
        Console.SetCursorPosition(0, 0);
        var ymax = 22;
        var xmax = 45;
        var yoff = 0;
        var xoff = 0;
        for (var y = 0; y < ymax; y++)
        {
            for (var x = 0; x < xmax; x++)
                if (map.ContainsKey((x, y)))
                    Console.Write(map[(x, y)] switch
                    {
                        1 => "██",
                        2 => "░░",
                        3 => "==",
                        4 => "()",
                        _ => "  "
                    });
            Console.WriteLine();
        }
    }
}