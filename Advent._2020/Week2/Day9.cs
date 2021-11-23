using System;
using System.IO;
using System.Linq;
using Advent.Core_2019_2020;

namespace Advent._2020.Week2;

public class Day9 : Day<long[], long>
{
    protected override long[] ReadData()
    {
        return File.ReadAllLines("Week2/input9.txt")
            .Select(long.Parse)
            .ToArray();
    }

    protected override long TaskA()
    {
        var d = 25;
        for (var i = d; i < Input.Length; i++)
            if (!IsSumOfTwo(Input[i], Input[(i - d)..i]))
            {
                wrongIndex = i;
                wrongValue = Input[i];
                return Input[i];
            }

        return -1;
    }

    private int wrongIndex;
    private long wrongValue;

    protected override long TaskB()
    {
        var (from, to) = FindSumRange(wrongValue, wrongIndex);
        var s = Input[from..to].OrderBy(x => x).ToArray();
        return s[0] + s[^1];
    }

    private bool IsSumOfTwo(long val, ReadOnlySpan<long> arr)
    {
        for (var i = 0; i < arr.Length; i++)
        for (var j = 0; j < arr.Length; j++)
            if (i != j && arr[i] + arr[j] == val)
                return true;
        return false;
    }

    private (int from, int to) FindSumRange(long value, int index)
    {
        var sum = Input[0];
        var from = 0;
        var to = 0;

        while (from < index)
            if (sum < value)
                sum += Input[++to];
            else if (sum > value)
                sum -= Input[from++];
            else if (sum == value)
                return (from, to);

        return (-1, -1);
    }
}