namespace Advent._2024.Week1;

public class Day1 : IDay
{
    private readonly int[] _left;
    private readonly int[] _right;

    public Day1(string[] input)
    {
        _left = new int[input.Length];
        _right = new int[input.Length];

        for (var i = 0; i < input.Length; i++)
        {
            var line = input[i];
            var split = line.Split("   ");
            _left[i] = int.Parse(split[0]);
            _right[i] = int.Parse(split[1]);
        }

        Array.Sort(_left);
        Array.Sort(_right);
    }

    public object Part1() => _left.Zip(_right, (l, r) => Math.Abs(l - r)).Sum();

    public object Part2()
    {
        var right = _right.CountBy(x => x).ToDictionary(x => x.Key, x => x.Value);
        return _left.Sum(left => Math.Abs(left * right.GetValueOrDefault(left)));
    }
}