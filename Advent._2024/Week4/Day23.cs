namespace Advent._2024.Week4;
using Set = HashSet<string>;
using Graph = Dictionary<string, HashSet<string>>;

public class Day23(string[] input) : IDay
{
    private readonly Graph _graph = CreateGraph(input);

    public object Part1() => GetPcSets().Distinct().Count();

    public object Part2() => string.Join(",",
        BronKerboschRecursive(_graph, [], [.._graph.Keys], [])
            .OrderByDescending(x => x.Count)
            .First()
            .Order());

    private IEnumerable<ThreePcs> GetPcSets() =>
        from pc0 in _graph
        from pc1 in pc0.Value
        from pc2 in pc0.Value
        where pc1 != pc2
        where pc0.Key.StartsWith('t') || pc1.StartsWith('t') || pc2.StartsWith('t')
        where _graph[pc1].Contains(pc0.Key) && _graph[pc2].Contains(pc0.Key) && _graph[pc1].Contains(pc2)
        select ThreePcs.CreateSorted(pc0.Key, pc1, pc2);

    private static IEnumerable<Set> BronKerboschRecursive(Graph graph, Set r, Set p, Set x)
    {
        if (p.Count == 0 && x.Count == 0)
        {
            // R is a maximal clique
            yield return [..r];
            yield break;
        }

        var pCopy = new Set(p);
        foreach (var vertex in pCopy)
        {
            r.Add(vertex);
            var neighbors = graph.TryGetValue(vertex, out var value) ? value : [];
            var newP = new Set(p.Intersect(neighbors));
            var newX = new Set(x.Intersect(neighbors));

            foreach (var enumerable in BronKerboschRecursive(graph, r, newP, newX))
            {
                yield return enumerable;
            }

            r.Remove(vertex);
            p.Remove(vertex);
            x.Add(vertex);
        }
    }

    private static Graph CreateGraph(string[] input) => input
        .SelectMany(x => new[] { (a: x[..2], b: x[3..]), (a: x[3..], b: x[..2]) })
        .GroupBy(x => x.a)
        .ToDictionary(x => x.Key, x => x.Select(y => y.b).ToHashSet());

    private record ThreePcs(string X, string Y, string Z)
    {
        public static ThreePcs CreateSorted(string x, string y, string z)
        {
            if (string.CompareOrdinal(x, y) > 0) (x, y) = (y, x);
            if (string.CompareOrdinal(y, z) > 0) (y, z) = (z, y);
            if (string.CompareOrdinal(x, y) > 0) (x, y) = (y, x);
            return new ThreePcs(x, y, z);
        }
    }
}