using Advent.Core;

namespace Advent._2021.Week2;

internal class Day8 : IReadInputDay
{
    public (char[][] signals, char[][] output)[] Input { get; set; }

    public void ReadData()
    {
        Input = File.ReadAllLines("Week2/Day8.txt")
            .Select(x => x.Split(new[] { " ", "|" }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            .Select(x => (signals: x[..10], output: x[10..]))
            .Select(x=> (
                x.signals.Select(y => y.OrderBy(c => c).ToArray()).OrderBy(y => y.Length).ToArray(), 
                x.output.Select(y => y.OrderBy(c => c).ToArray()).ToArray()))
            .ToArray();
    }

    public object TaskA() => Input.Sum(z => z.output.Count(y => y.Length is <= 4 or 7));

    public object TaskB() => Input.Select(XD).Sum();

    int XD((char[][] signals, char[][] output) input)
    {
        var s = input.signals;
        
        var _1 = s[0];
        var _7 = s[1];
        var _4 = s[2];
        var _8 = s[^1];
        var _235 = s[3..6];
        var _690 = s[6..9];
        var _4_exc1 = _4.Except(_1).ToArray();

        var _5 = _235.First(x => x.Intersect(_4_exc1).Count() == _4_exc1.Length);
        var _3 = _235.First(x => x != _5 && x.Intersect(_1).Count() == _1.Length);
        var _2 = _235.First(x => x != _3 && x != _5);

        var _9 = _690.First(x => x.Intersect(_3).Count() == _3.Length);
        var _6 = _690.First(x => x != _9 && x.Intersect(_5).Count() == _5.Length);
        var _0 = _690.First(x => x != _6 && x != _9);

        var n = input.output.Select(x =>
        {
            if (x.SequenceEqual(_1)) return 1;
            if (x.SequenceEqual(_2)) return 2;
            if (x.SequenceEqual(_3)) return 3;
            if (x.SequenceEqual(_4)) return 4;
            if (x.SequenceEqual(_5)) return 5;
            if (x.SequenceEqual(_6)) return 6;
            if (x.SequenceEqual(_7)) return 7;
            if (x.SequenceEqual(_8)) return 8;
            if (x.SequenceEqual(_9)) return 9;
            if (x.SequenceEqual(_0)) return 0;
            return -1;
        }).ToArray();

        return int.Parse(string.Join("", n));
    }
}
