using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Advent._2019.Intcode;
using Advent.Core_2019_2020;

#pragma warning disable 8509

namespace Advent._2019.Week2;

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
        var map = new Dictionary<Vec2, int>();
        var computer = new Computer(ints);

        var output = 0;
        var rotations = new[] {(0, 1), (1, 0), (0, -1), (-1, 0)};
        var rot = 0;
        Vec2 pos = 0;

        var sb = new StringBuilder();
        computer.Input.Enqueue(0);
        int i = 0;
        computer.OnProgramOutput += (s, e) =>
        {
            switch (output)
            {
                case 0:
                    output = 1;
                    return;
                case 1:
                    i++;
                    output = 0;
                    sb.AppendLine($"===============================")
                        .AppendLine($"iter: {i}");
                    sb.AppendLine($"I am on {pos}");

                    map[pos] = (int) computer.Output[^2]; // kolorowanko
                    sb.AppendLine($"colored here to {map[pos]}");

                    //obliczenie nowego obrotu
                    rot = (rot + ((int) computer.Output[^1] * 2 - 1) + rotations.Length) % rotations.Length;
                    var prevpos = pos; // tymczasowe
                    pos += rotations[rot]; // ruch w zadanym kierunku
                    sb.AppendLine($"I went to --> {pos}");

                    var inn = map.ContainsKey(pos) ? map[pos] : 0; // input
                    sb.AppendLine($"new input color: {inn} at {pos}");
                    computer.Input.Enqueue(inn);
                    return;
            }
        };

        computer.Compute();
        File.WriteAllText("pos.txt", sb.ToString());

        for (var y = 0; y >= -6; y--, Console.WriteLine()) // no braces!
        for (var x = 0; x < 40; x++)
            Console.Write(map.ContainsKey((x, y)) && map[(x, y)] == 1 ? "X" : " ");

        return map.Count;
    }
}