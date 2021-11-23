using System;
using System.IO;
using System.Linq;

namespace Advent._2019.Week1;

public class Day1
{
    public static void Execute()
    {
        //task 1
        var lines = File.ReadLines(@"Week1\input1.txt")
            .Select(int.Parse)
            .ToArray();
        var result = lines.Select(GetFuel).Sum();
        Console.WriteLine(result);

        //task 2
        var result2 = lines.Select(Recursively).Sum();
        Console.WriteLine(result2);
    }

    public static int GetFuel(int mass)
    {
        return mass > 6 ? mass / 3 - 2 : 0; // mass > 6 .... equivalent to Math.Max(0, ..);
    }

    public static int Recursively(int mass)
    {
        if (mass <= 0)
            return 0;
        return GetFuel(mass) + Recursively(GetFuel(mass));
    }
}