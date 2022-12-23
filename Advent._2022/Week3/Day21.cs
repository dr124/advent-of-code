using Advent.Core;
using BenchmarkDotNet.Characteristics;

namespace Advent._2022.Week3;

public class Day21 : IReadInputDay
{
    private static Monke[] _input;
    private static Dictionary<string, Monke> _dict;

    private record Monke
    {
        public string Name { get; private init; }
        public double? _value;
        public char Op;

        public double Yell()
        {
            if (_value is not null)
            {
                return _value.Value;
            }

            return Op switch
            {
                '*' => _dict[M1].Yell() * _dict[M2].Yell(),
                '/' => _dict[M1].Yell() / _dict[M2].Yell(),
                '+' => _dict[M1].Yell() + _dict[M2].Yell(),
                '-' => _dict[M1].Yell() - _dict[M2].Yell(),
            };
        }
        
        public static Monke Create(string str)
        {
            // nzhf: fjzn * bnws
            var name = str[..4];
            if (str.Length == 17)
            {
                var m1 = str[6..10];
                var m2 = str[13..];
                var op = str[11];
                return new Monke
                {
                    Name = name,
                    M1 = m1,
                    M2 = m2,
                    Op = op,
                };
            }
            else
            {
                var num = str[6..];
                return new Monke
                {
                    Name = name,
                    _value = double.Parse(num),
                };
            }
        }

        public string M2 { get; set; }

        public string M1 { get; set; }
    }

    public void ReadData()
    {
        _input = File.ReadLines("Week3/Day21.txt")
            .Select(Monke.Create)
            .ToArray();
        _dict = _input.ToDictionary(x => x.Name);

        var xd = new Solution();
        var all = xd.PartOne(File.ReadAllText("Week3/Day21.txt"));
        var all2 = xd.PartTwo(File.ReadAllText("Week3/Day21.txt"));
    }

    public object? TaskA()
    {
        return _dict["root"].Yell();
    }

    public object? TaskB()
    {
        var human = _dict["humn"];
        human._value = 111;

        var root = _dict["root"];
        var r1 = _dict[root.M1];
        var r2 = _dict[root.M2];

        var x1 = -500000;
        var x2 = 5000000;
        human._value = x1;
        var y1 = r1.Yell();
        human._value = x2;
        var y2 = r1.Yell();

        var a = (y2 - y1) / (x2 - x1);
        var b = y2 - a * x2;

        var y = a * r2.Yell() + b;
        var wanted = (y - b) / a;
        return wanted;
    }
}

// 80526799293735 too high
// 1189209258 too low



class Solution
{

    public object PartOne(string input)
    {
        return Parse(input, "root", false).Simplify();
    }

    public object PartTwo(string input)
    {
        var expr = Parse(input, "root", true) as Eq;

        while (!(expr.left is Var))
        {
            expr = Solve(expr);
        }
        return expr.right;
    }

    // One step in rearranging the equation to <variable> = <constant> form.
    // It is supposed that there is only one variable occurrence in the whole 
    // expression tree.
    Eq Solve(Eq eq) =>
        eq.left switch
        {
            Op(Const l, "+", Expr r) => new Eq(r, new Op(eq.right, "-", l).Simplify()),
            Op(Const l, "*", Expr r) => new Eq(r, new Op(eq.right, "/", l).Simplify()),
            Op(Expr l, "+", Expr r) => new Eq(l, new Op(eq.right, "-", r).Simplify()),
            Op(Expr l, "-", Expr r) => new Eq(l, new Op(eq.right, "+", r).Simplify()),
            Op(Expr l, "*", Expr r) => new Eq(l, new Op(eq.right, "/", r).Simplify()),
            Op(Expr l, "/", Expr r) => new Eq(l, new Op(eq.right, "*", r).Simplify()),
            Const => new Eq(eq.right, eq.left),
            _ => eq
        };

    // parses the input including the special rules for part2 
    // and returns the expression with the specified name
    Expr Parse(string input, string name, bool part2)
    {

        var context = new Dictionary<string, string[]>();
        foreach (var line in input.Split("\n"))
        {
            var parts = line.Split(" ");
            context[parts[0].TrimEnd(':')] = parts.Skip(1).ToArray();
        }

        Expr buildExpr(string name)
        {
            var parts = context[name];
            if (part2)
            {
                if (name == "humn")
                {
                    return new Var("humn");
                }
                else if (name == "root")
                {
                    return new Eq(buildExpr(parts[0]), buildExpr(parts[2]));
                }
            }
            if (parts.Length == 1)
            {
                return new Const(long.Parse(parts[0]));
            }
            else
            {
                return new Op(buildExpr(parts[0]), parts[1], buildExpr(parts[2]));
            }
        }

        return buildExpr(name);
    }

    // standard expression tree representation
    interface Expr
    {
        Expr Simplify();
    }

    record Const(long Value) : Expr
    {
        public override string ToString() => Value.ToString();
        public Expr Simplify() => this;
    }

    record Var(string name) : Expr
    {
        public override string ToString() => name;
        public Expr Simplify() => this;
    }

    record Eq(Expr left, Expr right) : Expr
    {
        public override string ToString() => $"{left} == {right}";
        public Expr Simplify() => new Eq(left.Simplify(), right.Simplify());
    }

    record Op(Expr left, string op, Expr right) : Expr
    {
        public override string ToString() => $"({left}) {op} ({right})";
        public Expr Simplify()
        {
            return (left.Simplify(), op, right.Simplify()) switch
            {
                (Const l, "+", Const r) => new Const(l.Value + r.Value),
                (Const l, "-", Const r) => new Const(l.Value - r.Value),
                (Const l, "*", Const r) => new Const(l.Value * r.Value),
                (Const l, "/", Const r) => new Const(l.Value / r.Value),
                (Expr l, _, Expr r) => new Op(l, op, r),
            };
        }
    }
}