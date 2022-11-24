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
}