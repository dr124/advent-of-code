using Advent.Core;

namespace Advent._2022.Week1;

public class Day3 : IReadInputDay
{
    private string[] _input;
    
    public void ReadData() => _input 
        = File.ReadAllLines("Week1/Day3.txt");

    public object TaskA() => _input
        .Select(x => new[] { x[..(x.Length / 2)], x[(x.Length / 2)..] })
        .Select(CommonCharacter)
        .Select(Priority)
        .Sum();

    public object TaskB() => _input
        .Chunk(3)
        .Select(CommonCharacter)
        .Select(Priority)
        .Sum();
    
    private static char CommonCharacter(params string[] str) => str
        .SelectMany(x => x.Distinct())
        .GroupBy(x => x)
        .MaxBy(x => x.Count())!
        .Key;

    private static int Priority(char c) => c switch
    {
        >= 'a' and <= 'z' => c - 'a' + 1,
        >= 'A' and <= 'Z' => c - 'A' + 27,
        _ => throw new Exception("what")
    };
}