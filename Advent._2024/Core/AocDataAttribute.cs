namespace Advent._2024.Core;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class AocDataAttribute(string path, object? part1 = null, object? part2 = null)
    : TestCaseAttribute(
        new AocData(File.Exists(path)
                ? File.ReadAllLines(path)
                : null,
            part1,
            part2,
            path));