using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Advent.Core;

namespace Advent._2021.Week3;

internal class Day21 : IReadInputDay
{
    private readonly Dictionary<State, Result> _dict = new();
    private readonly (int value, int number)[] AllRolls = (
            from i in Enumerable.Range(1, 3)
            from j in Enumerable.Range(1, 3)
            from k in Enumerable.Range(1, 3)
            let roll = i + j + k
            select roll)
        .GroupBy(x => x)
        .Select(x => (x.Key, x.Count()))
        .ToArray();

    public void ReadData()
    {
    }
    private static int Mod10(int a) => (a - 1) % 10 + 1;


    public object TaskA()
    {
        var score1 = 0;
        var score2 = 0;
        var pos1 = 7;
        var pos2 = 2;
        var rolled = 0;
        while (true)
        {
            //p1
            var r1 = ++rolled + ++rolled + ++rolled;
            pos1 = Mod10(pos1 + r1);
            score1 += pos1;

            if (score1 >= 1000)
                return score2 * rolled;

            //p2
            var r2 = ++rolled + ++rolled + ++rolled;
            pos2 = Mod10(pos2 + r2);
            score2 += pos2;

            if (score2 >= 1000)
                return score1 * rolled;
        }
    }

    public object TaskB() => Play(new State(true, 7, 2, 0, 0)).Max();

    private Result Play(State state) => state switch
    {
        { Score1: >= 21 } => new(1, 0),
        { Score2: >= 21 } => new(0, 1),
        _ => AllRolls.Aggregate(Result.Zero, (result, roll) => result + PlayCached(NextState(state, roll.value)) * roll.number)
    };

    private State NextState(State s, int roll) =>
        new(!s.IsPlayer1,
            s.IsPlayer1 ? Mod10(s.Pos1 + roll) : s.Pos1,
            s.IsPlayer1 ? s.Pos2 : Mod10(s.Pos2 + roll),
            s.IsPlayer1 ? s.Score1 + Mod10(s.Pos1 + roll) : s.Score1,
            s.IsPlayer1 ? s.Score2 : s.Score2 + Mod10(s.Pos2 + roll));

    private Result PlayCached(State s) => _dict.TryGetValue(s, out var res) ? res : _dict[s] = Play(s);

    // ===============
    private readonly record struct Result(long A, long B)
    {
        public static Result Zero => new(0, 0);
        public long Max() => A > B ? A : B;
        public static Result operator +(Result a, Result b) => new(a.A + b.A, a.B + b.B);
        public static Result operator *(Result a, int b) => new(a.A * b, a.B * b);
    }

    private record struct State(bool IsPlayer1, int Pos1, int Pos2, int Score1, int Score2);
}