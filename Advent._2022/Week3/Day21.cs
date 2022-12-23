using Advent.Core;

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
        var r1 = _dict[root.M1].Yell();
        var r2 = _dict[root.M2].Yell();

        var x1 = 0;
        var x2 = 10000000;
        human._value = x1;
        var y1 = root.Yell();
        human._value = x2;
        var y2 = root.Yell();

        var a = (y2 - y1) / (x2 - x1);
        var b = y1 - a * x1;

        var y = a * 1 + b;

        //var wanted = r2;
        //var x = (wanted * b) / a;
        return -1;
    }
}

// 1189209258 too low