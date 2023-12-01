global using Advent._2023.Core;
using Advent._2023.Week1;
using Xunit;

namespace Advent._2023;

public class Main
{
    [Theory]
    [AocData("Week1/Example1-1.txt", part1: 142)]
    [AocData("Week1/Example1-2.txt", part2: 281)]
    [AocData("Week1/Day1.txt", part1: 52974, part2: 53340)]
    public void Day1Tests(string[] data, object? part1, object? part2) => 
        AocTester.Test<Day1>(data, part1, part2);
}