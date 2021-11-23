using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Advent.Core_2019_2020;
using PCRE;

namespace Advent._2020.Week3;

public class Day19 : Day<string[], int>
{
    private const string INPUT_FILE = "Week3/input19.txt";

    protected override string[] ReadData() =>
        File.ReadLines(INPUT_FILE)
            .SkipWhile(x => x != "")
            .Skip(1)
            .ToArray();

    protected override int TaskA()
    {
        var lines = File.ReadLines(INPUT_FILE)
            .TakeWhile(x => x != "");
        var regex = CreateRegex(lines);
        return Input.Count(x => regex.IsMatch(x));
    }

    protected override int TaskB()
    {
        var lines = File.ReadLines(INPUT_FILE)
            .TakeWhile(x => x != "")
            .Select(x =>
            {
                if (x.StartsWith("8:"))
                    return "8: 42 | 42 8";
                if (x.StartsWith("11:"))
                    return "11: 42 31 | 42 11 31";
                return x;
            });
            
        var regex = CreateRegex(lines);
        return Input.Count(x => regex.IsMatch(x));
    }

    // ============================

    private PcreRegex CreateRegex(IEnumerable<string> lines)
    {
        const string rulesStart = "(?(DEFINE)";
        const string rulesEnd = ")";
        const string startGroupFormat = "(?<_{0}>";
        const string groupRefFormat = "(?&_{0})";
        const string terminatingFormat = "({0})";
        const string end = ")^(?&_0)$";

        var sb = new StringBuilder();
        sb.Append(rulesStart);

        foreach (var line in lines.OrderBy(x => x))
        {
            var split = line.Split(": ");
            var index = split[0];
            sb.AppendFormat(startGroupFormat, index);
                
            var isTerminating = split[1].Contains("\"");
            if (isTerminating)
            {
                var letter = split[1].Remove("(\")*");
                sb.AppendFormat(terminatingFormat, letter);
            }
            else
            {
                var groups = split[1].Split(" | ");
                for (var i = 0; i < groups.Length; i++)
                {
                    foreach (var gr_ref in groups[i].SplitClear(" "))
                        sb.AppendFormat(groupRefFormat, gr_ref);
                    if (i < groups.Length - 1)
                        sb.Append('|');
                }
            }

            sb.AppendLine(rulesEnd);
        }

        sb.Append(end);
        return new PcreRegex(sb.ToString());
        // based on https://stackoverflow.com/questions/58735015/
    }
}