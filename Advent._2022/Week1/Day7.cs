using Advent.Core;

namespace Advent._2022.Week1;

public class Day7 : IReadInputDay
{
    private static List<Node> _allNodes = new();
    private static Node _root = new(null, "/", null);
    
    public void ReadData()
    {
        void InsertNode(Node parent, Node newNode)
        {
            parent.Children.Add(newNode);
            _allNodes.Add(newNode);
        }

        var pointer = _root;
        foreach (var line in File.ReadLines("Week1/Day7.txt"))
        { 
            var arg = line.Split(' ');
            if (arg[0] is "$")
            {
                if (arg[1] is "cd")
                {
                    pointer = pointer[arg[2]];
                }
            }
            else if (arg[0] is "dir")
            {
                InsertNode(pointer, new Node(pointer, arg[1], null));
            }
            else
            {
                InsertNode(pointer, new Node(pointer, arg[1], long.Parse(arg[0])));
            }
        }
    }

    public object? TaskA()
    {
        return _allNodes
            .Where(x => x.Size is null)
            .Select(x => x.TotalSize())
            .Where(x => x <= 100000)
            .Sum();
    }

    public object? TaskB()
    {
        var totalSum = _root.TotalSize();
        var disk = 70000000;
        var needed = 30000000;
        var remain = disk - totalSum;
        var minSizeToDelete = needed - remain;

        return _allNodes
            .Where(x => x.Size is null)
            .Select(x => x.TotalSize())
            .Where(x => x >= minSizeToDelete)
            .Min();
    }

    public record Node(Node Parent, string Name, long? Size) 
    {
        public List<Node> Children { get; } = new();
        private long? _size = null;
        public long TotalSize() => _size ??= Size ?? Children.Sum(c => c.TotalSize());
        public Node this[string name] => name switch
        {
            ".." => Parent,
            "/" => _root,
            "." => this,
            _ => Children.First(c => c.Name == name)
        };
    }
}