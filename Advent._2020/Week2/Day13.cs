using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Advent.Core_2019_2020;

namespace Advent._2020.Week2;

public class Day13 : Day<int[], long>
{
    private int?[] lines;
    private int time;

    protected override int[] ReadData()
    {
        var file = File.ReadAllLines("Week2/input13.txt");

        time = int.Parse(file[0]);
        lines = file[1].Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(x => x == "x" ? null : (int?) int.Parse(x))
            .ToArray();

        return new int[0];
    }

    protected override long TaskA()
    {
        var smallest_time = int.MaxValue;
        var smallest_line = int.MaxValue;
        foreach (var line in lines.Where(x => x != null))
        {
            var linetime = line.Value * (int) Math.Ceiling((float) time / line.Value);
            if (linetime < smallest_time)
            {
                smallest_line = line.Value;
                smallest_time = linetime;
            }
        }

        return (smallest_time - time) * smallest_line;
    }

    protected override long TaskB()
    {
        var b = new List<long>();
        var n = new List<long>();

        for (var i = 0; i < lines.Length; i++)
            if (lines[i] != null)
            {
                b.Add(lines[i].Value - i);
                n.Add(lines[i].Value);
            }

        var N = n.ProductLong();
        var Ni = n.Select(x => N / x).ToList();

        var xi = Ni.Select((_, i) =>
        {
            var mult = (int) Math.Floor((double) Ni[i] / n[i]);
            var a = Ni[i] - mult * n[i];

            for (var j = 1;; j++)
                if (a * j % n[i] == 1)
                    return j;
        }).ToList();

        var m = xi.Select((_, i) => b[i] * Ni[i] * xi[i]).ToList();
        var end = m.Sum() % N;

        return end;
    }
}