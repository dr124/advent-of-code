using System;
using System.IO;
using System.Linq;
using Advent.Core_2019_2020;

namespace Advent._2020.Week1;

public class Day6 : Day<string[], int>
{
    protected override string[] ReadData()
    {
        var str = File.ReadAllText("Week1/input6.txt");

        var str_split = str.Replace("\r", "")
            .Replace("\n\n", "@")
            .Replace("\n", " ")
            .Replace("  ", " ")
            .Split("@", StringSplitOptions.RemoveEmptyEntries);

        return str_split;
    }

    protected override int TaskA()
    {
        return Input.Sum(str =>
            str.Remove("\\s")
                .GroupBy(c => c)
                .Count());
    }

    protected override int TaskB()
    {
        return Input.Sum(str =>
        {
            var n = str.Split(' ').Length;
            var answers = str.Remove("\\s").GroupBy(c => c);
            return answers.Count(c => c.Count() == n);
        });
    }
}