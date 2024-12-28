namespace Advent._2024.Week4;

public class Day22(string[] input) : IDay
{
    private readonly SequenceCache[] _sequences = [..input.Select(CreateSequence)];

    public object Part1() => _sequences.Sum(x => x.Final);

    public object Part2() => _sequences
        .SelectMany(x => x)
        .GroupBy(x => x.Key)
        .Select(x => x.Sum(y => y.Value))
        .Max();

    private static SequenceCache CreateSequence(string number)
    {
        int secret = int.Parse(number), prevDiff1 = 0, prevDiff2 = 0, prevDiff3 = 0;
        var sequences = new SequenceCache();

        for (var i = 0; i < 2000; i++)
        {
            var newSecret = CalculateSecret(secret);
            var diff = newSecret % 10 - secret % 10;

            if (i >= 3)
            {
                var sequence = new Sequence(prevDiff3, prevDiff2, prevDiff1, diff);
                sequences.TryAdd(sequence, newSecret % 10);
            }

            prevDiff3 = prevDiff2;
            prevDiff2 = prevDiff1;
            prevDiff1 = diff;
            secret = newSecret;
        }

        sequences.Final = secret;
        return sequences;
    }

    private static int CalculateSecret(int s0)
    {
        const int prune = 0xffffff;
        var s1 = (s0 ^ (s0 << 6)) & prune;
        var s2 = (s1 ^ (s1 >> 5)) & prune;
        var s3 = (s2 ^ (s2 << 11)) & prune;
        return s3;
    }

    private record Sequence(int A, int B, int C, int D);

    private class SequenceCache : Dictionary<Sequence, int>
    {
        public long Final { get; set; }
    }
}