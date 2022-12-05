using Advent.Core;
using Advent.Core.Extensions;

namespace Advent._2022.Week1;

public class Day5 : IReadInputDay
{
    private char[][] _map;
    private Operation[] _operations;
    
    public void ReadData()
    {
        var lines = File.ReadAllLines("Week1/Day5.txt");
        var split = Array.IndexOf(lines, "");

        _map = lines[..(split - 1)]
            .To2dArray()
            .Transpose()
            .Where(x => char.IsLetter(x[^1]))
            .Select(x => x.Reverse().Where(c => c is not ' '))
            .To2dArray();
        
        _operations = lines[(split + 1)..]
            .Select(StringUtils.ExtractNumbers)
            .Select(x => new Operation(x[0], x[1] - 1, x[2] - 1))
            .ToArray();
    }

    public object TaskA() => Process(containers => containers);

    public object TaskB() => Process(containers => containers.Reverse());

    private string Process(Func<IEnumerable<char>, IEnumerable<char>> transform)
    {
        var stack = _map.Select(x => new Stack<char>(x)).ToArray();

        foreach (var (count, from, to) in _operations)
        {
            stack[to].PushMany(transform(stack[from].PopMany(count)));
        }

        return new string(stack.Select(x => x.Peek()).ToArray());
    } 

    private record Operation(int Count, int From, int To);
}