namespace Advent._2024.Week3;

public class Day19(string[] input) : IDay
{
    private readonly Dictionary<ReadOnlyMemory<char>, long> _results = [];
    private readonly List<ReadOnlyMemory<char>> _patterns = input[2..].Select(x => x.AsMemory()).ToList();
    private readonly List<ReadOnlyMemory<char>> _rules = input[0]
        .Split(',', StringSplitOptions.TrimEntries)
        .Select(x => x.AsMemory())
        .ToList();
    
    public object Part1() => _patterns.Count(pattern => Check(pattern) > 0);
    
    public object Part2() => _patterns.Sum(Check);

    // this solution got benchmarked and found to be 5x faster than plain string
    // using spans/memory decreased time from ~125ms to ~25ms
    private long Check(ReadOnlyMemory<char> pattern)
    {
        if (_results.TryGetValue(pattern, out var result))
        {
            return result;
        }
        
        long r = 0;
        foreach (var rule in _rules)
        {
            if (pattern.Length == 0) return 1;
            if (pattern.Span.StartsWith(rule.Span)) 
                r += Check(pattern[rule.Length..]);
        }

        return _results[pattern] = r;
    }
}