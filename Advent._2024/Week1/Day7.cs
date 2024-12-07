using System.Numerics;

namespace Advent._2024.Week1;

public class Day7(string[] input) : IDay
{
    private readonly HashSet<Equation> _equations = input.Select(Equation.Parse).ToHashSet();
    private long _resultPart1;
    public object Part1()
    {
        long sum = 0;
        foreach (var equation in _equations)
        {
            if (IsTrue(equation.Result, equation.Numbers[0], equation.Numbers.AsSpan()[1..]))
            {
                _equations.Remove(equation);
                sum += equation.Result;
            }
        }

        return _resultPart1 = sum;
    }

    public object Part2() => _equations
        // .AsParallel()
        .Where(equation => IsTrue2(equation.Result, equation.Numbers[0], equation.Numbers.AsSpan()[1..]))
        .Sum(equation => equation.Result) + _resultPart1;


    private bool IsTrue(long result, long value, Span<int> elements)
    {
        if (elements.Length == 0)
        {
            return value == result;
        }

        if (value > result)
        {
            return false;
        }
        
        return IsTrue(result, value * elements[0], elements[1..]) 
               || IsTrue(result, value + elements[0], elements[1..]);
    }

    private bool IsTrue2(long result, long value, Span<int> elements)
    {
        if (elements.Length == 0)
        {
            return value == result;
        }

        if (value > result)
        {
            return false;
        }

        return IsTrue2(result, value + elements[0], elements[1..])
               || IsTrue2(result, value * elements[0], elements[1..])
               || IsTrue2(result, MathExtensions.Concat(value, elements[0]), elements[1..]);
    }

    private record Equation(long Result, int[] Numbers)
    {
        public static Equation Parse(string equation)
        {
            var split = equation.Split(':');
            var numbers = split[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return new Equation(long.Parse(split[0]), numbers.Select(int.Parse).ToArray());
        }
    }
}