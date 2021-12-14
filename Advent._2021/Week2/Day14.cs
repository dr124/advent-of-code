using Advent.Core;
using looong = System.Int64;
namespace Advent._2021.Week2;

internal class Day14 : IReadInputDay
{
    private Dictionary<string, string> Production;
    private string Input;

    public void ReadData()
    {
        var lines = File.ReadAllLines("Week2/Day14.txt");
        Input = lines[0];
        Production = lines[2..].Select(x => x.Split(" -> ")).ToDictionary(x => x[0], x => x[1]);
    }

    public object TaskA() => Produce(10);
    public object TaskB() => Produce(40);

    looong Produce(int epochs)
    {
        var produced = Production.Select(x => x.Value).Distinct().ToDictionary(x => x, _ => 0L);
        var cache = Production.ToDictionary(x => x.Key, _ => 0L);

        for (var i = 1; i < Input.Length; i++) 
            cache[Input[(i - 1)..(i + 1)]]++;

        for (var i = 0; i < Input.Length; i++) 
            produced[Input.Substring(i, 1)]++;

        for (var e = 0; e < epochs; e++)
        {
            var c = cache.ToDictionary(x => x.Key, x => x.Value);
            foreach (var (str, n) in cache)
            {
                var prod = Production[str]; 
                c[str] -= n;
                c[str[0] + prod] += n;
                c[prod + str[1]] += n;
                produced[prod] += n;
            }
            cache = c;
        }

        var max = produced.Max(x => x.Value);
        var min = produced.Min(x => x.Value);
        return max - min;
    }
}