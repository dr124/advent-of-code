using System.Text;

namespace Advent._2023.Week2;

public class Day12(string[] input) : IDay
{
    private Arrangement[] _arrangements = input
        .Select(Arrangement.ParseLine)
        .ToArray();

    public object Part1() => _arrangements
        .AsParallel()
        .Select(CalculatePossibleArrangements)
        .Sum();

    private long CalculatePossibleArrangements(Arrangement arg)
    {
        return Xd("", arg.Pattern, arg.Rules);
    }

    private long Xd(string currentPattern, ReadOnlySpan<char> remainingPattern, int[] rules)
    {
        var ruleParts = currentPattern.Split('.', StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Length)
            .ToArray();

        if (remainingPattern.Length == 0)
        {
            //check if rules are correct
            if (rules.SequenceEqual(ruleParts))
            {
                return 1;
            }

            return 0;
        }

        var partsFound = ruleParts.Length;
        if (partsFound > 1)
        {
            var x = partsFound - 1;
            if (x < rules.Length)
            {
                var check = ruleParts[..x];
                var check2 = rules[..x];
                if (!check.SequenceEqual(check2))
                {
                    return 0;
                }
            }
        }

        var currentChar = remainingPattern[0];

        if (currentChar == '?')
        {
            var remaining = remainingPattern[1..];
            var withDot = Xd(currentPattern + '.', remaining, rules);
            var withHash = Xd(currentPattern + '#', remaining, rules);
            return withDot + withHash;
        }
        else
        {
            var questionIndex = remainingPattern.IndexOf('?');
            var nonQuestion = questionIndex > 0 
                ? remainingPattern[..questionIndex] 
                : remainingPattern;
            var remaining = remainingPattern[nonQuestion.Length..];

            return Xd(currentPattern + nonQuestion.ToString(), remaining, rules);
        }

    }

    private int inow = 0;
    private int imax = input.Length;
    public object Part2() => _arrangements
        .Select(x => new Arrangement(
            string.Join('?', Enumerable.Repeat(x.Pattern, 5)),
            [.. x.Rules, .. x.Rules, .. x.Rules, .. x.Rules, .. x.Rules]))
        .Select((arrangement, i) =>
        {
            var res = CalculatePossibleArrangements(arrangement);
            inow++;
            Console.WriteLine($"finish {i} -> {(float)inow/imax:P2}");
            return res;
        })
        .Sum();

    private record Arrangement(string Pattern, int[] Rules)
    {
        public static Arrangement ParseLine(string line)
        {
            var parts = line.Split(' ');
            return new Arrangement(parts[0], parts[1].Split(',').Select(int.Parse).ToArray());
        }
    }
}