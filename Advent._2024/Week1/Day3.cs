namespace Advent._2024.Week1;

public class Day3(string[] input) : IDay
{
    private const string MultiplyPattern = "mul(";
    private const string DoPattern = "do()";
    private const string DontPattern = "don't()";
    private bool _enabled = true;
    
    public object Part1() => Calculate(false);
    
    public object Part2() => Calculate(true);
    
    private object Calculate(bool isEnablingEnabled)
    {
        long sum = 0;
        var line = string.Join("", input);
        var span = line.AsSpan();
        for (var i = 0; i < line.Length; i++)
        {
            if (isEnablingEnabled && _enabled && span[i..].StartsWith(DontPattern))
            {
                i += DontPattern.Length;
                _enabled = false;
            }
            
            if (isEnablingEnabled && !_enabled && span[i..].StartsWith(DoPattern))
            {
                i += DoPattern.Length;
                _enabled = true;
            }

            if (_enabled && span[i..].StartsWith(MultiplyPattern))
            {
                i += MultiplyPattern.Length;
                var mul = FindPattern(span, ref i);
                if (mul.HasValue)
                {
                    sum += mul.Value;
                }
            }
        }

        return sum;
    }

    private long? FindPattern(ReadOnlySpan<char> span, ref int i)
    {
        // Detect first number
        int? firstNum = FindNumber(span, ref i);
        if (!firstNum.HasValue)
        {
            return null;
        }

        // Detect comma
        if (span[i++] != ',')
        {
            return null;
        }

        // Detect second number
        int? secondNum = FindNumber(span, ref i);
        if (!secondNum.HasValue)
        {
            return null;
        }

        // Detect closing parenthesis
        if (span[i] != ')')
        {
            return null;
        }

        return firstNum.Value * secondNum.Value;
    }

    private int? FindNumber(ReadOnlySpan<char> span, ref int i)
    {
        int? num = null;
        for (; i < span.Length; i++)
        {
            if (char.IsDigit(span[i]))
            {
                num ??= 0;
                num *= 10;
                num += span[i] - '0';
            }
            else
            {
                break;
            }
        }

        return num;
    }
}