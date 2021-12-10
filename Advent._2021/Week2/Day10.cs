using Advent.Core;
using Advent.Core.Extensions;

namespace Advent._2021.Week2;

internal class Day10 : IReadInputDay
{
    public string[] Input { get; set; }

    public void ReadData() => Input = File.ReadAllLines("Week2/Day10.txt");

    public object TaskA() => Input.Select(XD).Sum();
    public object TaskB() => Input.Select(XD2).Where(x => x > 0).Median();

    int XD(string str)
    {
        Stack<char> q = new();

        foreach (var c in str)
            if (IsOpening(c)) q.Push(c);
            else if (GetClosing(q.Pop()) != c)
                return c switch
                {
                    ')' => 3,
                    ']' => 57,
                    '}' => 1197,
                    '>' => 25137
                };
        
        return 0;
    }
    
    long XD2(string str)
    {
        var isCorrupted = false;
        Stack<char> q = new();

        foreach (var c in str)
            if (IsOpening(c)) q.Push(c);
            else if (GetClosing(q.Pop()) != c) 
                isCorrupted = true;

        if(!isCorrupted)
            return q.Aggregate<char, long>(0, (sum, c) => sum * 5 + c switch
            {
                '(' => 1,
                '[' => 2,
                '{' => 3,
                '<' => 4
            });

        return 0;
    }

    bool IsOpening(char c) => c is '(' or '[' or '{' or '<';
    char GetClosing(char c) =>
        c switch
        {
            '(' => ')',
            '[' => ']',
            '{' => '}',
            '<' => '>'
        };

}