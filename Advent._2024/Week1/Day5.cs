using System.Collections.ObjectModel;

namespace Advent._2024.Week1;

public class Day5 : IDay
{
    private readonly int[][] _allUpdates;
    private readonly List<int[]> _incorrectUpdates = [];
    private readonly Dictionary<int, HashSet<int>> _nextPages;

    public Day5(string[] input)
    {
        var mid = input.Select((str, idx) => (str, idx))
            .Where(x => x.str == string.Empty)
            .Select(x => x.idx)
            .First();

        _nextPages = input[..mid]
            .Select(ParseRule)
            .GroupBy(x => x.number)
            .ToDictionary(x => x.Key, x => x.Select(y => y.before).ToHashSet());

        _allUpdates = input[++mid..]
            .Select(ParseUpdate)
            .ToArray();
    }
   
    public object Part1()
    {
        var result = 0;
        foreach (var update in _allUpdates)
        {
            if (CheckUpdate(update))
            {
                result += update[update.Length / 2];
            }
            else
            {
                _incorrectUpdates.Add(update);
            }
        }
        
        return result;
    }
    
    public object Part2()
    {
        var sum = 0;
        foreach (var input in _incorrectUpdates)
        {
            Array.Sort(input, CompareUpdates);
            sum += input[input.Length / 2];
        }
        
        return sum;
    }

    private bool CheckUpdate(int[] update)
    {
        for (var i = update.Length - 1; i >= 1; i--)
        {
            var currentPage = update[i];
            var alreadyPrinted = update[..i];

            if (_nextPages.TryGetValue(currentPage, out var nextPages) 
                && alreadyPrinted.Any(nextPages.Contains))
            {
                return false;
            }
        }

        return true;
    }

    private int CompareUpdates(int x, int y)
    {
        if (_nextPages.TryGetValue(x, out var nextPagesForX) && nextPagesForX.Contains(y))
        {
            return -1; 
        }

        if (_nextPages.TryGetValue(y, out var nextPagesForY) && nextPagesForY.Contains(x))
        {
            return 1;
        }

        return 0;
    }

    private static (int number, int before) ParseRule(string input)
    {
        // always 2 digits numbers
        return (int.Parse(input[..2]), int.Parse(input[3..]));
    }
    
    private static int[] ParseUpdate(string x)
    {
        return x.Split(',').Select(int.Parse).ToArray();
    }
}