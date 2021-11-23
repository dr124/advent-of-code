using System.Collections.Generic;
using System.IO;
using System.Linq;
using Advent.Core_2019_2020;

namespace Advent._2020.Week3;

public class Day18 : Day<string[], long>
{
    protected override string[] ReadData() => File.ReadAllLines("Week3/Input18.txt");

    protected override long TaskA() => Input.Select(x => CalculateA(x.ToList())).Sum();

    protected override long TaskB() => Input.Select(x => CalculateB(x.ToList())).Sum();

    // ============================
        
    public long CalculateA(List<char> line)
    {
        var sum = 0L;

        while (HasParenthesis(line.ToArray().ToList(), out var xd))
        {
            var inner = line.GetRange(xd.from, xd.count);
            var result = CalculateA(inner);
            line.RemoveRange(xd.from - 1, xd.count + 2);
            line.InsertRange(xd.from - 1, result.ToString());
        }

        var elements = new string(line.ToArray()).SplitClear(" ").ToList();

        var op = "+";
        while (elements.Count != 0)
        {
            var el = elements[0];
            if (el is not ("+" or "*"))
                sum = op switch
                {
                    "+" => sum + long.Parse(el),
                    "*" => sum * long.Parse(el)
                };
            else
                op = el;
            elements.RemoveAt(0);
        }

        return sum;
    }

    public long CalculateB(List<char> line)
    {
        while (HasParenthesis(line, out var xd))
        {
            var inner = line.GetRange(xd.from, xd.count);
            var result = CalculateB(inner);
            line.RemoveRange(xd.from - 1, xd.count + 2);
            line.InsertRange(xd.from - 1, result.ToString());
        }

        var elements = new string(line.ToArray()).SplitClear(" ").ToList();

        while (HasSymbol(elements, "+", out var add))
        {
            var a = long.Parse(elements[add - 1]);
            var b = long.Parse(elements[add + 1]);
            elements[add - 1] = (a + b).ToString();
            elements.RemoveRange(add, 2);
            line = string.Join(" ", elements).ToList();
        }

        while (HasSymbol(elements, "*", out var add))
        {
            var a = long.Parse(elements[add - 1]);
            var b = long.Parse(elements[add + 1]);
            elements[add - 1] = (a * b).ToString();
            elements.RemoveRange(add, 2);
            line = string.Join(" ", elements).ToList();
        }

        return long.Parse(line.ToArray());
    }

    private bool HasParenthesis(List<char> line, out (int from, int count) parenthesis)
    {
        parenthesis = (0, 0);

        var parStart = line.IndexOf('(');
        if (parStart != -1)
        {
            parStart += 1;
            var parEnd = parStart;
            for (var p = 1; p != 0; parEnd++)
                if (line[parEnd] == '(') p += 1;
                else if (line[parEnd] == ')') p -= 1;
            parEnd -= 1;
            parenthesis = (parStart, parEnd - parStart);
            return true;
        }

        return false;
    }

    private bool HasSymbol(List<string> line, string symbol, out int index)
    {
        index = line.IndexOf(symbol);
        return index != -1;
    }
}