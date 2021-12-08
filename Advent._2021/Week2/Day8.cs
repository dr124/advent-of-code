using Advent.Core;

namespace Advent._2021.Week2;

internal class Day8 : IReadInputDay
{
    public (string[] signals, string[] output)[] Input { get; set; }

    public void ReadData()
    {
        Input = File.ReadAllLines("Week2/Day8.txt")
            .Select(x => x.Split(new[] { " ", "|" }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            .Select(x => (signals: x[..10], output: x[10..]))
            .Select(x=> (
                x.signals.Select(y => new string(y.OrderBy(c => c).ToArray())).OrderBy(y => y.Length).ToArray(), 
                x.output.Select(y => new string(y.OrderBy(c => c).ToArray())).ToArray()))
            .ToArray();
    }

    public object TaskA() => Input.Sum(z => z.output.Count(y => y.Length is <= 4 or 7));

    public object TaskB() => Input
        .Select(x => XDD(x.signals, x.output))
        .Select(x => int.Parse(string.Join("", x)))
        .Sum();

    int XDD(string[] signals, string[] output)
    {
        var parser = ParseSignals(signals);
        var numbers = output.Select(parser);
        return int.Parse(string.Join("", numbers));
    }

    Func<string, int> ParseSignals(string[] signals)
    {
        var _1 = signals[0];
        var _7 = signals[1];
        var _4 = signals[2];
        var _8 = signals[^1];
        var _235 = signals[3..6];
        var _690 = signals[6..9];
        var _4_exc1 = _4.Except(_1).ToArray();

        var _5 = _235.First(x => x.Intersect(_4_exc1).Count() == _4_exc1.Length);
        var _3 = _235.First(x => x != _5 && x.Intersect(_1).Count() == _1.Length);
        var _2 = _235.First(x => x != _3 && x != _5);

        var _9 = _690.First(x => x.Intersect(_3).Count() == _3.Length);
        var _6 = _690.First(x => x != _9 && x.Intersect(_5).Count() == _5.Length);
        var _0 = _690.First(x => x != _6 && x != _9);

        return s =>
        {
            if (s == _1) return 1; if (s == _2) return 2;
            if (s == _3) return 3; if (s == _4) return 4;
            if (s == _5) return 5; if (s == _6) return 6;
            if (s == _7) return 7; if (s == _8) return 8;
            if (s == _9) return 9; if (s == _0) return 0;
            return -1;
        };
    }

}
