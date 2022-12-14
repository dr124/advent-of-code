using System.Text.RegularExpressions;

namespace Advent.Core.Extensions;

public static partial class StringUtils
{
    [GeneratedRegex("\\d+")]
    private static partial Regex MatchNumbersRegex();
    
    public static int[] ExtractNumbers(this string str) =>
        MatchNumbersRegex().Matches(str)
            .Select(x => int.Parse(x.Value))
            .ToArray();
    
    public static T[][] To2dArray<T>(this IEnumerable<IEnumerable<T>> input) =>
        input.Select(x => x.ToArray()).ToArray();
}