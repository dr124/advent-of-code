using System.Numerics;
using System.Runtime.CompilerServices;

namespace Advent._2023.Week2;

public class Day13 : IDay
{
    List<Pattern> _patterns = [];

    public Day13(string[] input)
    {
        var patternLines = new List<string>();
        foreach (var line in input)
        {
            if (!string.IsNullOrWhiteSpace(line))
            {
                patternLines.Add(line);
            }
            else if (patternLines.Count > 0)
            {
                _patterns.Add(Pattern.Parse(patternLines));
                patternLines = [];
            }
        }
        if (patternLines.Count > 0)
        {
            _patterns.Add(Pattern.Parse(patternLines));
        }
    }

    public object Part1() => _patterns.Sum(x => DoTheThing(x, false));

    public object Part2() => _patterns.Sum(x => DoTheThing(x, true));

    private static int DoTheThing(Pattern pattern, bool isSmudgy)
    {
        var mirrorH = FindMirror(pattern.Horizontal.AsSpan(), isSmudgy);
        if (mirrorH != null)
        {
            return mirrorH.Value * 100;
        }

        var mirrorV = FindMirror(pattern.Vertical.AsSpan(), isSmudgy);
        if (mirrorV != null)
        {
            return mirrorV.Value;
        }

        throw new Exception("No mirror found");
    }

    private static int? FindMirror(ReadOnlySpan<uint> tab, bool isSmudgy)
    {
        for (var i = 1; i < tab.Length; i++)
        {
            var firstPart = tab[..i];
            var secondPart = tab[i..];
            var hasSmudge = !isSmudgy;
            for (var j = 0; ; j++)
            {
                var diff = firstPart[^(j + 1)] ^ secondPart[j];
                if (diff != 0)
                {
                    if (!hasSmudge && uint.IsPow2(diff))
                    {
                        hasSmudge = true;
                    }
                    else
                    {
                        break;
                    }
                }

                if (j == firstPart.Length - 1 || j == secondPart.Length - 1)
                {
                    if (hasSmudge)
                    {
                        return i;
                    }

                    break;
                }
            }
        }

        return null;
    }

    private record Pattern(uint[] Horizontal, uint[] Vertical)
    {
        public static Pattern Parse(IReadOnlyList<string> lines)
        {
            var horizontal = new uint[lines.Count];
            var vertical = new uint[lines[0].Length];

            for (var i = 0; i < lines.Count; i++)
            {
                var line = lines[i];
                for (var j = 0; j < line.Length; j++)
                {
                    var c = line[j];
                    if (c == '#')
                    {
                        horizontal[i] |= (uint)(1 << j);
                        vertical[j] |= (uint)(1 << i);
                    }
                }
            }

            return new Pattern(horizontal, vertical);
        }
    }
}