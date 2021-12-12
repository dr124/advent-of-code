using Advent.Core;

namespace Advent._2021.Week2;

internal class Day12 : IReadInputDay
{
    private Dictionary<string, string[]> dict;

    bool IsVisitOnce(string str) => str[0] >= 'a';

    public void ReadData()
    {
        var lines = File.ReadAllLines("Week2/Day12.txt");
        var pairs = lines.Select(x => x.Split('-')).Select(x => (from: x[0], to: x[1])).ToArray();
        var names = pairs.SelectMany(x => new[] { x.from, x.to }).Distinct();

        dict = names.ToDictionary(
            x => x,
            x => Array.Empty<string>()
                .Concat(pairs.Where(y => y.from == x).Select(y => y.to))
                .Concat(pairs.Where(y => y.to == x).Select(y => y.from))
                .Except(new[] { "start" })
                .ToArray());
    }

    public object TaskA() => Traverse(new() { "start" }, true);
    public object TaskB() => Traverse(new() { "start" }, false);

    int Traverse(List<string> visited, bool hasVisitedTwice)
    {
        var here = visited[^1];
        if (here == "end")
            return 1;

        var toVisit = dict[here];
        if (hasVisitedTwice)
            toVisit = toVisit.Except(visited.Where(IsVisitOnce)).ToArray();

        return (from node in toVisit
                let isTwiceNow = hasVisitedTwice || IsVisitOnce(node) && visited.Contains(node)
                select Traverse(visited.Append(node).ToList(), isTwiceNow))
            .Sum();
    }
}