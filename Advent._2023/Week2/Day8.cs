namespace Advent._2023.Week2;

public class Day8 : IDay
{
    private static readonly char[] separator = ['(', ')', ' ', ',', '='];
    private readonly string _seq;
    private readonly Dictionary<string, Node> _nodes;

    public Day8(string[] input)
    {
        _seq = input[0];
        _nodes = input[2..]
            .Select(Node.Parse)
            .ToDictionary(x => x.Key);

        foreach (var node in _nodes.Values)
        {
            node.Left = _nodes[node.LeftKey];
            node.Right = _nodes[node.RightKey];
        }
    }

    public object Part1() => Process(_nodes["AAA"], key => key == "ZZZ");

    public object Part2() => _nodes.Values
            .Where(x => x.Key.EndsWith('A'))
            .Select(node => Process(node, key => key.EndsWith('Z')))
            .Aggregate(1L, Lcm);

    private long Process(Node node, Func<string, bool> hasFinished)
    {
        for (long i = 0;; i++)
        {
            var dir = _seq[(int)(i % _seq.Length)];
            node = dir switch
            {
                'L' => node.Left,
                'R' => node.Right,
                _ => throw new Exception()
            };

            if (hasFinished(node.Key))
            {
                return i + 1;
            }
        }
    }

    // greatest common divisor 
    static long Gcd(long a, long b)
    {
        while (b != 0)
        {
            long temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    // least common multiple
    static long Lcm(long a, long b)
    {
        return (a / Gcd(a, b)) * b;
    }

    private record Node(string Key, string LeftKey, string RightKey)
    {
        public Node Left { get; set; }
        public Node Right { get; set; }

        public static Node Parse(string line)
        {
            var parts = line.Split(separator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            return new Node(parts[0], parts[1], parts[2]);
        }
    }
}