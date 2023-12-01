namespace Advent._2023.Week1;

public class Day1(string[] lines) : IDay
{
    public object TaskA() => DoTheThing(false);

    public object TaskB() => DoTheThing(true);

    private int DoTheThing(bool useText) => lines
            .Select(line => GetFirstDigit(line, useText) * 10 + GetLastDigit(line, useText))
            .Sum();

    private int GetFirstDigit(string line, bool useText)
    {
        for (var i = 0; i < line.Length; i++)
        {
            if (IsDigit(line.AsSpan()[i..], useText, out var digit))
            {
                return digit;
            }
        }

        throw new Exception("no digit bro");
    }

    private int GetLastDigit(string line, bool useText)
    {
        for (var i = line.Length - 1; i >= 0; i--)
        {
            if (IsDigit(line.AsSpan()[i..], useText, out var digit))
            {
                return digit;
            }
        }
        
        throw new Exception("no digit bro");
    }

    private bool IsDigit(ReadOnlySpan<char> line, bool useText, out int digit)
    {
        if (char.IsDigit(line[0]))
        {
            digit = line[0] - '0';
            return true;
        }

        if (useText)
        {
            foreach (var (text, d) in _digits)
            {
                if (line.StartsWith(text))
                {
                    digit = d;
                    return true;
                }
            }
        }

        digit = 0;
        return false;
    }

    private readonly (string, int)[] _digits =
    [
        ("one", 1),
        ("two", 2),
        ("three", 3),
        ("four", 4),
        ("five", 5),
        ("six", 6),
        ("seven", 7),
        ("eight", 8),
        ("nine", 9)
    ];
}