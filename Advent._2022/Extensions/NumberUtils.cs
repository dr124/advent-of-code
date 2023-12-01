namespace Advent.Core.Extensions;

public static class NumberUtils
{
    public static bool IsBetween(this int val, int a, int b) =>
        a < b
            ? val >= a && val <= b
            : val >= b && val <= a;

    public static int Product(this IEnumerable<int> arr) => arr.Aggregate(1, (a, b) => a * b);
}