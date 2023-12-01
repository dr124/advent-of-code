using System.Reflection;

namespace Advent.Core;

public interface IDay
{
    object TaskA();
    object TaskB();
}

public interface IReadInputDay : IDay
{
    void ReadData();
}

public interface IReadInputDay<out T> : IReadInputDay
{
    T Input { get; }
}

public static class DayFactory
{
    public static IDay GetDay(int day, Assembly assembly)
    {
        var type = assembly.DefinedTypes.First(x => x.Name.Equals($"Day{day}"));
        return Activator.CreateInstance(type) as IDay ?? throw new InvalidOperationException();
    }
}