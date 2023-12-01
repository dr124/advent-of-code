using System.Reflection;
using Xunit.Sdk;

namespace Advent._2023.Core;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class AocData(string path, object? part1 = null, object? part2 = null) : DataAttribute
{
    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"no example file bro ({path})");
        }

        yield return [File.ReadAllLines(path), part1, part2];
    }
}