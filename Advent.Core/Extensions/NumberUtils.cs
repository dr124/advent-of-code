namespace Advent.Core.Extensions;

public static class NumberUtils
{
    public static bool IsBetween<T>(this T val, T a, T b) where T : INumber<T> =>
        a < b
            ? val >= a && val <= b
            : val >= b && val <= a;
}