using Advent.Core;
using Advent.Core.Extensions;
using Microsoft.Diagnostics.Tracing.Parsers.Kernel;

namespace Advent._2021.Week3;

internal class Day18 : IReadInputDay
{
    static Branch topNode;

    interface INode
    {
        int Magnitude();
        bool Reduce(int n);
    }

    class Leaf : INode
    {
        public Leaf? Next;
        public Leaf? Prev;
        public int Value;

        public int Magnitude() => Value;
        
        public bool Reduce(int n = 0)
        {
            return false;
        }

        public override string ToString() => Value.ToString();
    }

    class Branch : INode
    {
        public INode Left;
        public INode Right;

        private bool ReduceInternal(int n)
        {
            BuildLinkedList(topNode);

            {
                if (Left is Branch lb)
                {
                    if (lb.ReduceInternal(n + 1)) return true;
                }
            }
            {
                if (Left is Leaf { Value: > 9 } lLeaf)
                {
                    Left = new Branch
                    {
                        Left = new Leaf { Value = (int)Math.Floor(lLeaf.Value / 2f) },
                        Right = new Leaf { Value = (int)Math.Ceiling(lLeaf.Value / 2f) }
                    };
                    Console.WriteLine($"Splitting leaf {lLeaf} at {this}");
                    BuildLinkedList(topNode);
                    return true;
                }
            }
            if (n >= 3)
            {
                if (Left is Branch lb)
                {
                    if (lb.Right is Leaf { Next: { } } rLeaf)
                        rLeaf.Next.Value += rLeaf.Value;

                    if (lb.Left is Leaf { Prev: { } } lLeaf)
                        lLeaf.Prev.Value += lLeaf.Value;

                    Left = new Leaf { Value = 0 };
                    BuildLinkedList(topNode);
                    Console.WriteLine($"Exploding branch {lb}");
                    return true;
                }
            }
            {
                if (Right is Branch rb)
                {
                    if (rb.ReduceInternal(n + 1)) return true;
                }
            }
            {
                if (Right is Leaf { Value: > 9 } rLeaf)
                {
                    Right = new Branch
                    {
                        Left = new Leaf { Value = (int)Math.Floor(rLeaf.Value / 2f) },
                        Right = new Leaf { Value = (int)Math.Ceiling(rLeaf.Value / 2f) }
                    };
                    Console.WriteLine($"Splitting leaf {rLeaf} at {this}");
                    BuildLinkedList(topNode);
                    return true;
                }
            }
            if (n >= 3)
            {

                if (Right is Branch rb)
                {
                    if (rb.Right is Leaf { Next: { } } rLeaf)
                        rLeaf.Next.Value += rLeaf.Value;

                    if (rb.Left is Leaf { Prev: { } } lLeaf)
                        lLeaf.Prev.Value += lLeaf.Value;

                    Right = new Leaf { Value = 0 };
                    Console.WriteLine($"Exploding branch {rb}");
                    BuildLinkedList(topNode);
                    return true;
                }
            }

            return false;
        }

        public bool Reduce(int n)
        {
            while (ReduceInternal(n))
                Console.WriteLine("\t"+this);
            return false;
        }
        
        public int Magnitude() => Left.Magnitude() * 3 + Right.Magnitude() * 2;
        public override string ToString() => $"[{Left},{Right}]";
    }

    public void ReadData()
    {
        var lines = File.ReadAllLines("Week3/Day18.txt");
        var nodes = lines.Select(BuildNode).Cast<Branch>().ToArray();
        topNode = nodes.First();
        topNode.Reduce(0);

        foreach (var n in nodes.Skip(1))
        {
            //n.Reduce(0);
            topNode = AddNode(topNode, n);
            topNode.Reduce(0);
            topNode.Reduce(0);
            topNode.Reduce(0);
            topNode.Reduce(0);
            Console.WriteLine($"=  {topNode}");
            Console.WriteLine();
        }

        Console.WriteLine($"{topNode} => {topNode.Magnitude()}");
    }

    static INode BuildNode(string str)
    {
        for (int i = 0, brackets = 0;; i++)
        {
            if (str[i] == '[') brackets++;
            else if (str[i] == ']') brackets--;
            else if (brackets == 0 && str[i] is <= '9' and >= '0')
                return new Leaf { Value = str[i] - '0' };
            else if (str[i] == ',' && brackets == 1)
            {
                var left = BuildNode(str[1..i]);
                var right = BuildNode(str[(i + 1)..^1]);
                var newNode = new Branch { Left = left, Right = right };
                return newNode;
            }
        }
    }

    static void BuildLinkedList(INode node)
    {
        static void xd(INode node, List<Leaf> nodes)
        {
            if (node is Leaf leaf)
            {
                nodes.Add(leaf);
                return;
            }

            var branch = (Branch)node;
            xd(branch.Left, nodes);
            xd(branch.Right, nodes);
        }

        var list = new List<Leaf>();
        xd(node, list);
        for (int i = 1; i < list.Count - 1; i++)
        {
            list[i].Prev = list[i - 1];
            list[i - 1].Next = list[i];
            list[i].Next = list[i + 1];
            list[i + 1].Prev = list[i];
        }
    }

    static Branch AddNode(Branch n1, Branch n2)
    {
        Console.WriteLine();
        Console.WriteLine($"   {n1}");
        Console.WriteLine($"+  {n2}");

        var node = new Branch
        {
            Left = n1,
            Right = n2
        };
        return node;
    }

    public object TaskA() => null;

    public object TaskB() => null;
}