namespace Advent._2023.Week3;

public class Day18(string[] input) : IDay
{
    public object Part1() => Calculate(ParsePart1);

    public object Part2() => Calculate(ParsePart2);

    private long Calculate(Parsing parsing)
    {
        // parse input
        var n = input.Length;
        var totalLength = 0;
        var plans = new (int dx, int dy)[n];
        for (var i = 0; i < input.Length; i++)
        {
            plans[i] = parsing(input[i]);
            totalLength += Math.Abs(plans[i].dx + plans[i].dy);
        }

        // create points
        var pts = new (long x, long y)[n + 1];
        for (var i = 0; i < plans.Length; i++)
        {
            pts[i + 1] = (pts[i].x + plans[i].dx, pts[i].y + plans[i].dy);
        }

        // calculate area
        long area = 0;
        for (var j = 0; j < n; j++)
        {
            area += pts[j].x * pts[j + 1].y - pts[j + 1].x * pts[j].y;
        }

        return (Math.Abs(area) + totalLength) / 2 + 1;
    }

    private static (int, int) ParsePart1(string line)
    {
        var d = int.Parse(line[2..4]);
        return line[0] switch
        {
            'R' => (d, 0),
            'L' => (-d, 0),
            'U' => (0, d),
            'D' => (0, -d),
            _ => throw new Exception("Unknown dir")
        };
    }

    private static (int, int) ParsePart2(string line)
    {
        var hex = line[^7..^2];
        var d = Convert.ToInt32(hex, 16);
        var dir = line[^2];
        return dir switch
        {
            '0' => (d, 0),
            '1' => (0, d),
            '2' => (-d, 0),
            '3' => (0, -d),
            _ => throw new Exception("Unknown dir")
        };
    }

    private delegate (int dx, int dy) Parsing(string line);
}