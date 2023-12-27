namespace Advent._2023.Week3;

public class Day19 : IDay
{
    private readonly Dictionary<string, Rule> _rules = [];
    private readonly List<Xmas> _parts = [];

    public Day19(string[] input)
    {
        // rules
        int i = 0;
        for (; input[i].Length > 0; i++)
        {
            var line = input[i];
            var ruleStart = line.IndexOf('{');
            var ruleName = line[..ruleStart];
            var allRules = line[(ruleStart + 1)..^1];
            _rules[ruleName] = Rule.Parse(allRules);
        }

        // parts
        i++;
        for (; i < input.Length; i++)
        {
            var p = input[i].Split('=', ',');
            var x = int.Parse(p[1]);
            var m = int.Parse(p[3]);
            var a = int.Parse(p[5]);
            var s = int.Parse(p[7][..^1]);
            _parts.Add(new Xmas(x, m, a, s));
        }
    }

    public object Part1() => _parts.Where(ExecuteRules).Sum(x => x.X + x.M + x.A + x.S);

    public object Part2()
    {
        var start = new XmasRange(1, 4000, 1, 4000, 1, 4000, 1, 4000, "in");

        List<XmasRange> approved = [];
        Queue<XmasRange> queue = [];
        queue.Enqueue(start);

        while (queue.TryDequeue(out var range))
        {
            foreach (var subrange in XD(range))
            {
                if (subrange.Label is "A")
                {
                    approved.Add(subrange);
                }
                else if (subrange.Label is not "R")
                {
                    queue.Enqueue(subrange);
                }
            }
        }

        return approved.Sum(x => x.Size());
    }

    private IEnumerable<XmasRange> XD(XmasRange range)
    {
        var rule = _rules[range.Label];

        foreach (var subrule in rule.Rules)
        {
            var ranges = XD2(range, subrule);
            range = ranges.old;
            yield return ranges.@new;
        }

        yield return range with { Label = rule.Final };
    }

    private (XmasRange @new, XmasRange old) XD2(XmasRange r, Subrule s)
    {
        return (s.Op, s.Variable) switch
        {
            ('<', 'x') => (r with { ToX = s.Value - 1, Label = s.OnSuccess }, r with { FromX = s.Value }),
            ('<', 'm') => (r with { ToM = s.Value - 1, Label = s.OnSuccess }, r with { FromM = s.Value }),
            ('<', 'a') => (r with { ToA = s.Value - 1, Label = s.OnSuccess }, r with { FromA = s.Value }),
            ('<', 's') => (r with { ToS = s.Value - 1, Label = s.OnSuccess }, r with { FromS = s.Value }),
            ('>', 'x') => (r with { FromX = s.Value + 1, Label = s.OnSuccess }, r with { ToX = s.Value }),
            ('>', 'm') => (r with { FromM = s.Value + 1, Label = s.OnSuccess }, r with { ToM = s.Value }),
            ('>', 'a') => (r with { FromA = s.Value + 1, Label = s.OnSuccess }, r with { ToA = s.Value }),
            ('>', 's') => (r with { FromS = s.Value + 1, Label = s.OnSuccess }, r with { ToS = s.Value }),
            _ => throw new Exception("Unknown operator")
        };
    }


    private bool ExecuteRules(Xmas xmas)
    {
        var rule = _rules["in"];
        while (true)
        {
            var result = rule.Execute(xmas);
            if (result is "R" or "A")
            {
                return result is "A";
            }

            rule = _rules[result];
        }
    }

    private record Rule(Subrule[] Rules, string Final)
    {
        public string Execute(Xmas xmas)
        {
            foreach (var rule in Rules)
            {
                var result = rule.Execute(xmas);
                if (result is not null)
                {
                    return result;
                }
            }

            return Final;
        }

        public static Rule Parse(string allRules)
        {
            var subrules = allRules.Split(',');
            return new Rule(subrules[..^1].Select(Subrule.Parse).ToArray(), subrules[^1]);
        }
    }

    private record Subrule(char Variable, char Op, int Value, string OnSuccess)
    {
        public string? Execute(Xmas xmas)
        {
            var value = Variable switch
            {
                'x' => xmas.X,
                'm' => xmas.M,
                'a' => xmas.A,
                's' => xmas.S,
                _ => throw new Exception("Unknown variable")
            };

            var result = Op switch
            {
                '<' => value < Value,
                '>' => value > Value,
                _ => throw new Exception("Unknown operator")
            };

            return result ? OnSuccess : null;
        }

        public static Subrule Parse(string str)
        {
            var variable = str[0];
            var op = str[1];
            var indexOf = str.IndexOf(':');
            var amount = int.Parse(str[2..indexOf]);
            var onSuccess = str[(indexOf + 1)..];

            return new Subrule(variable, op, amount, onSuccess);
        }
    }

    private record Xmas(int X, int M, int A, int S);

    private record XmasRange(int FromX, int ToX, int FromM, int ToM, int FromA, int ToA, int FromS, int ToS, string Label)
    {
        public long Size() => (ToX - FromX + 1L) 
                              * (ToM - FromM + 1L)
                              * (ToA - FromA + 1L)
                              * (ToS - FromS + 1L);
    }
}