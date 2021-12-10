using Advent.Core;
using Advent.Core.Extensions;

namespace Advent._2021.Week2;

internal class Day10 : IReadInputDay
{
    public string[] Input { get; set; }

    public void ReadData() => Input = File.ReadAllLines("Week2/Day10.txt");

    public object TaskA() => Input.Select(GetFirstCorrupted).Sum();
    public object TaskB() => Input.Select(GetCompletionCost).Where(x => x > 0).Median();

    int GetFirstCorrupted(string line)
    {
        Stack<char> queue = new();

        foreach (var c in line)
        {
            if (IsOpening(c)) queue.Push(c);
            else if (GetClosing(queue.Pop()) != c)
                return c switch
                {
                    ')' => 3,
                    ']' => 57,
                    '}' => 1197,
                    '>' => 25137
                };
        }

        return 0;
    }
    
    long GetCompletionCost(string line)
    {
        var isCorrupted = false;
        Stack<char> queue = new();

        foreach (var c in line)
        {
            if (IsOpening(c)) queue.Push(c);
            else if (GetClosing(queue.Pop()) != c)
                isCorrupted = true;
        }

        return isCorrupted
            ? 0 
            : queue.Aggregate<char, long>(0, (sum, c) => sum * 5 + c switch
            {
                '(' => 1,
                '[' => 2,
                '{' => 3,
                '<' => 4
            });
    }

    bool IsOpening(char c) => c is '(' or '[' or '{' or '<';
    char GetClosing(char c) => c switch
    {
        '(' => ')',
        '[' => ']',
        '{' => '}',
        '<' => '>'
    };
}