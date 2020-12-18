using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Advent.Core;
using Microsoft.VisualBasic;
using MoreLinq.Extensions;

namespace Advent._2020.Week3
{

    public class Day18 : Day<string[], long>
    {
        protected override string[] ReadData()
        {
            return File.ReadAllLines("Week3/Input18.txt").Select(x => x).ToArray();
        }

        protected override long TaskA()
        {
            return Input.Select(x => CalculateA(x.ToCharArray())).Sum();
        }

        protected override long TaskB()
        {
            return Input.Select(x => CalculateB(x.ToCharArray().ToList())).Sum();
        }

        public long CalculateA(Span<char> line)
        {
            var sum = 0L;

            while (FindParenthesis(new string(line), out var xd))
            {
                var inPar = CalculateA(line[xd.from..xd.to]);
                line[(xd.from - 1)..(xd.to + 1)].Fill(' ');
                foreach (var c in inPar.ToString())
                    line[xd.from++] = c;
            }

            var elements = new string(line).Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).ToList();

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
            var sum = 0L;

            while (true)
            {
                var parStart = line.IndexOf('(');
                if (parStart != -1)
                {
                    parStart += 1;
                    int parEnd = parStart;
                    for (int p = 1; p != 0; parEnd++)
                        if (line[parEnd] == '(') p += 1;
                        else if (line[parEnd] == ')') p -= 1;
                    parEnd -= 1;

                    var xdd = line.GetRange(parStart, parEnd - parStart);
                    var inPar = CalculateB(xdd);
                    line.RemoveRange(parStart - 1, parEnd - parStart + 2);
                    line.InsertRange(parStart - 1, inPar.ToString());
                }
                else
                    break;
            }

            while (true)
            {
                var elements = new string(line.ToArray()).SplitClear(" ").ToList();
                var add = elements.IndexOf("+");
                if (add != -1)
                {
                    var a = long.Parse(elements[add - 1]);
                    var b = long.Parse(elements[add + 1]);
                    elements[add - 1] = (a + b).ToString();
                    elements.RemoveRange(add, 2);
                    line = string.Join(" ", elements).ToList();
                }
                else
                    break;
            }


            while (true)
            {
                var elements = new string(line.ToArray()).SplitClear(" ").ToList();
                var mult = elements.IndexOf("*");
                if (mult != -1)
                {
                    var a = long.Parse(elements[mult - 1]);
                    var b = long.Parse(elements[mult + 1]);
                    elements[mult - 1] = (a * b).ToString();
                    elements.RemoveRange(mult, 2);
                    line = string.Join(" ", elements).ToList();
                }
                else
                    break;
            }

            return long.Parse(line.ToArray());
        }

        private bool FindParenthesis(ReadOnlySpan<char> line, out (int from,int to) parenthesis)
        {
            parenthesis = (0, 0);
            
            var parStart = line.IndexOf('(');
            if (parStart != -1)
            {
                parStart += 1;
                int parEnd = parStart;
                for (int p = 1; p != 0; parEnd++)
                    if (line[parEnd] == '(') p += 1;
                    else if (line[parEnd] == ')') p -= 1;
                parEnd -= 1;
                parenthesis = (parStart, parEnd);
                return true;
            }

            return false;
        }
    }
}
