namespace Advent.Core.Extensions;

public static class EnumerableUtils
{
    public static T Median<T>(this IEnumerable<T> tab) 
    {
        var sorted = tab.OrderBy(x => x).ToArray();
        var len = sorted.Length;
        if (len == 0)
            return default(T);
        return sorted[len / 2];
    }

    public static IEnumerable<T> Do<T>(this IEnumerable<T> source, Action<T> action)
    {
        foreach (var item in source)
        {
            action(item);
            yield return item;
        }
    }
}

public static class Enumerate
{
    public static IEnumerable<int> From(int start = 0)
    {
        for (int i = start; ; i++)
        {
            yield return i;
        }
    }
}