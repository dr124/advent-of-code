using System.Text.RegularExpressions;
using KeyPaths = System.Collections.Generic.Dictionary<(char, char), System.Collections.Generic.List<string>>;
using Keypad = System.Collections.Generic.Dictionary<char, Advent._2024.Core.Vec2<int>>;

namespace Advent._2024.Week3;

public class Day21(string[] input) : IDay
{
    private static readonly Keypad DigToVec = new()
    {
        ['7'] = (0, 0), ['8'] = (1, 0), ['9'] = (2, 0),
        ['4'] = (0, 1), ['5'] = (1, 1), ['6'] = (2, 1),
        ['1'] = (0, 2), ['2'] = (1, 2), ['3'] = (2, 2),
        ['0'] = (1, 3), ['A'] = (2, 3)
    };

    private static readonly Keypad DirToVec = new()
    {
        ['^'] = (1, 3), ['A'] = (2, 3),
        ['<'] = (0, 4), ['v'] = (1, 4), ['>'] = (2, 4)
    };

    private static readonly KeyPaths DigPaths = BuildKeypadPaths(DigToVec);
    private static readonly KeyPaths DirPaths = BuildKeypadPaths(DirToVec);

    public object Part1() => input.Select(x => Calc(x, 3)).Sum();

    public object Part2() => input.Select(x => Calc(x, 26)).Sum();

    private long Calc(string str, int depth)
    {
        var len = CalculateShortest(str, depth, depth);
        var dig = Regex.Replace(str, @"[^\d]", "");
        var num = int.Parse(dig);
        return len * num;
    }

    private readonly Dictionary<(string keys, int depth), long> _cache = [];

    private long CalculateShortest(string keys, int depth, int maxDepth)
    {
        if (depth == 0)
        {
            return keys.Length;
        }

        if (_cache.ContainsKey((keys, depth)))
        {
            return _cache[(keys, depth)];
        }

        var keysParts = keys.Split('A').SkipLast(1).Select(x => x + 'A');
        var keypad = depth != maxDepth ? DirPaths : DigPaths;
        return _cache[(keys, depth)] = keysParts
            .Sum(k => FindAllPossiblePaths(k, keypad).Min(s => CalculateShortest(s, depth - 1, maxDepth)));
    }

    private static List<string> FindAllPossiblePaths(string str, KeyPaths paths)
    {
        List<string> result = [];
        foreach (var (end, start) in str.Zip(str.Prepend('A')))
        {
            var p = paths[(start, end)];
            if (result.Count == 0)
            {
                result.AddRange(p.Select(r => r + 'A'));
            }
            else
            {
                result = (from r in result
                        from pp in p
                        select r + pp + 'A'
                    ).ToList();
            }
        }

        return result;
    }

    private static IEnumerable<string> BuildPathsOnKeypad(char start, char end, Keypad keypad)
    {
        var avoid = (0, 3);
        var queue = new Queue<(string Path, Vec2 Position)>();
        queue.Enqueue(("", keypad[start]));

        while (queue.TryDequeue(out var q))
        {
            var diff = keypad[end] - q.Position;
            if (diff == Vec2.Zero)
            {
                yield return q.Path;
                continue;
            }

            if (q.Position == avoid)
            {
                continue;
            }

            if (diff.X > 0)
            {
                queue.Enqueue((q.Path + '>', q.Position + Vec2.Right));
            }
            else if (diff.X < 0)
            {
                queue.Enqueue((q.Path + '<', q.Position + Vec2.Left));
            }

            if (diff.Y > 0)
            {
                queue.Enqueue((q.Path + 'v', q.Position + Vec2.Down));
            }
            else if (diff.Y < 0)
            {
                queue.Enqueue((q.Path + '^', q.Position + Vec2.Up));
            }
        }
    }

    private static KeyPaths BuildKeypadPaths(Keypad keypad) => keypad.Keys
        .SelectMany(_ => keypad.Keys, (start, end) => (start, end))
        .ToDictionary(pair => pair, pair => BuildPathsOnKeypad(pair.start, pair.end, keypad).ToList());
}