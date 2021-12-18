namespace Advent.Core.Extensions;

public static class EnumerableUtils
{
    public static T Median<T>(this IEnumerable<T> tab) where T : INumber<T>
    {
        var sorted = tab.OrderBy(x => x).ToArray();
        var len = sorted.Length;
        if (len == 0)
            return default(T);
        if (len % 2 == 0)
            return (dynamic)(sorted[len / 2 - 1] + sorted[len / 2]) / 2;
        return sorted[len / 2];
    }
}