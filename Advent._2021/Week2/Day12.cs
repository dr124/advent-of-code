using Advent.Core;
using Path = System.Collections.Generic.List<string>;

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

    public object TaskA() => Traverse(new() { "start" }, true).Count;
    public object TaskB() => Traverse(new() { "start" }, false).Count;

    List<Path> Traverse(Path visited, bool hasVisitedTwice)
    {
        var here = visited[^1];
        if (here == "end")
            return new() { visited };

        var toVisit = dict[here];
        if (hasVisitedTwice) 
            toVisit = toVisit.Except(visited.Where(IsVisitOnce)).ToArray();

        List<Path> visitedPaths = new();
        foreach (var node in toVisit)
        {
            var isTwiceNow = hasVisitedTwice || IsVisitOnce(node) && visited.Contains(node);
            var paths = Traverse(visited.Append(node).ToList(), isTwiceNow);
            visitedPaths.AddRange(paths);
        }

        return visitedPaths;
    }
}