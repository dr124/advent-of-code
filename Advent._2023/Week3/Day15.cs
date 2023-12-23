using System.Collections.Specialized;

namespace Advent._2023.Week3;

public class Day15(string[] input) : IDay
{
    private readonly LabelInfo[] _data = input[0].Split(',').Select(x => new LabelInfo(x)).ToArray();

    public object Part1() => _data.Sum(x => Hash(x.Label));

    private static int Hash(ReadOnlySpan<char> chars)
    {
        var val = 0;
        foreach (var c in chars)
        {
            val += c;
            val *= 17;
            val %= 256;
        }

        return val;
    }

    public object Part2()
    {
        var boxes = new List<Lens>[256];
        for (var i = 0; i < 256; i++)
            boxes[i] = [];

        foreach (var hm in _data)
        {
            var hash = Hash(hm.BaseName);
            var box = boxes[hash];
            var lens = box.FirstOrDefault(x => x.Name == hm.BaseName);
            if (lens == null)
            {
                if (hm.Operation == '=')
                {
                    lens = new Lens(hm.BaseName, hm.Number);
                    box.Add(lens);
                }
            }
            else
            {
                if (hm.Operation == '-')
                {
                    box.Remove(lens);
                }
                else
                {
                    lens.Number = hm.Number;
                }
            }
        }

        var sum = 0;
        for (var boxIndex = 0; boxIndex < boxes.Length; boxIndex++)
        {
            var box = boxes[boxIndex];
            for (var lensIndex = 0; lensIndex < box.Count; lensIndex++)
            {
                var lens = box[lensIndex];
                
                var a = 1 + boxIndex;
                var b = 1 + lensIndex;
                var c = lens.Number - '0';
                var d = a * b * c;
                
                sum += d;
            }
        }

        return sum;
    }

    private record LabelInfo
    {
        public string BaseName { get; }
        public char Operation { get; }
        public string Label { get; }
        public char Number { get; }

        public LabelInfo(string label)
        {
            Label = label;
            if (label[^1] == '-')
            {
                Operation = '-';
                BaseName = label[..^1];
                Number = label[^1];
            }
            else
            {
                Operation = '=';
                BaseName = label[..^2];
                Number = label[^1];
            }
        }
    }

    private record Lens(string Name, char Number)
    {
        public char Number { get; set; } = Number;
    }
}