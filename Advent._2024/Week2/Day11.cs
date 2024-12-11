using System.Collections.Concurrent;

namespace Advent._2024.Week2;

public class Day11(string[] input) : IDay
{
    private Dictionary<long, long> _stones = input[0]
        .Split(' ')
        .Select(int.Parse)
        .ToDictionary(x => (long)x, _ => 1L);

    public object Part1() => DoTheThing(25);

    public object Part2() => DoTheThing(75 - 25);

    private object DoTheThing(int blinks)
    {
        for (var i = 0; i < blinks; i++)
        {
            _stones = Blink(_stones);
        }
        
        return _stones.Values.Sum();
    }
    
    private static Dictionary<long, long> Blink(Dictionary<long, long> stones)
    {
        var d = new Dictionary<long, long>();
        
        foreach (var (value, count) in stones)
        {
            if (value == 0)
            {
                Add(d, 1, count);
                continue;
            }
            
            var str = value.ToString();
            if (str.Length % 2 == 0)
            {
                var left = long.Parse(str[..(str.Length / 2)]);
                var right = long.Parse(str[(str.Length / 2)..]);
                Add(d, left, count);
                Add(d, right, count);
                continue;
            }

            Add(d, value * 2024, count);
        }

        return d;
    }

    private static void Add(Dictionary<long, long> stones, long value, long count)
    {
        if (!stones.TryAdd(value, count))
        {
            stones[value] += count;
        }
    }
}