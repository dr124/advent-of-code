namespace Advent.Core.Extensions;

public static class SplitOptions
{
    public static StringSplitOptions Clear => StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries;
}