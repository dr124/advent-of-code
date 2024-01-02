using Iced.Intel;
using System.Text;

namespace Advent._2023.Week2;

public class Day12(string[] input) : IDay
{
    private Arrangement[] _arrangements = input
        .Select(Arrangement.ParseLine)
        .ToArray();

    public object Part1() => _arrangements
        .Select(x => Calculate(x.Pattern, x.Rules))
        .Sum();
    
    public object Part2() => _arrangements
        .Select(x => new Arrangement(
            string.Join('?', Enumerable.Repeat(x.Pattern, 5)),
            [.. x.Rules, .. x.Rules, .. x.Rules, .. x.Rules, .. x.Rules]))
        .Select(x => Calculate(x.Pattern, x.Rules))
        .Sum();

    Dictionary<string, long> _cache = [];

    private long Calculate(string pattern, int[] groups)
    {
        pattern = pattern.Trim();
        var key = $"{pattern}_{string.Join(',', groups)}";

        if (_cache.TryGetValue(key, out var value))
        {
            return value;
        }

        value = GetCount(pattern, groups);
        _cache[key] = value;

        return value;
    }

    private long GetCount(string pattern, int[] rules)
    {
        while (true)
        {
            if (rules.Length == 0)
            {
                return pattern.Contains('#') ? 0 : 1; 
            }

            if (string.IsNullOrEmpty(pattern))
            {
                return 0;
            }

            var start = pattern[0];

            if (start == '.')
            {
                pattern = pattern.Trim('.');
                continue;
            }

            if (start == '?')
            {
                var withDot = Calculate('.' + pattern[1..], rules);
                var withHash = Calculate('#' + pattern[1..], rules);
                return withDot + withHash;
            }

            if (start == '#') // Start of a group
            {
                if (rules.Length == 0
                    || pattern.Length < rules[0]
                    || pattern[..rules[0]].Contains('.'))
                {
                    return 0; 
                }

                if (rules.Length > 1)
                {
                    if (pattern.Length < rules[0] + 1 
                        || pattern[rules[0]] == '#')
                    {
                        return 0; 
                    }

                    pattern = pattern[(rules[0] + 1)..];
                    rules = rules[1..];
                    continue;
                }
                else
                {

                    pattern = pattern[rules[0]..];
                    rules = rules[1..];
                    continue;
                }
            }

            throw new Exception("Invalid input");
        }
    }


    private record Arrangement(string Pattern, int[] Rules)
    {
        public static Arrangement ParseLine(string line)
        {
            var parts = line.Split(' ');
            return new Arrangement(parts[0], parts[1].Split(',').Select(int.Parse).ToArray());
        }
    }
}