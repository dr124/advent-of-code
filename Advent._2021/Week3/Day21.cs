using System.Text;
using Advent.Core;
using Advent.Core.Extensions;
using BenchmarkDotNet.Attributes;

namespace Advent._2021.Week3;

internal class Day21 : IReadInputDay
{
    public void ReadData()
    {
      
    }

    int Mod10(int a) => (a - 1) % 10 + 1;

    public object TaskA()
    {
        int score1 = 0;
        int score2 = 0;
        int pos1 = 7;
        int pos2 = 2;
        int rolled = 0;
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


    int _pos1 = 7;
    int _pos2 = 2;

    private (int v,int n)[] rolls;
    public object TaskB()
    {
        rolls = (
                from i in Enumerable.Range(1, 3)
                from j in Enumerable.Range(1, 3)
                from k in Enumerable.Range(1, 3)
                let roll = i + j + k
                select roll)

            .GroupBy(x=>x)
            .Select(x=>(x.Key, x.Count()))
            .ToArray();
        var r = XD(true, 7, 2, 0, 0);
        return r.Item1 > r.Item2 ? r.Item1 : r.Item2;
    }
    

    public (long, long) XD(bool isP1, int pos1, int pos2, int score1, int score2)
    {
        if (score1 >= 21)
            return (1, 0);
        if (score2 >= 21) 
            return (0, 1);

        if (isP1)
        {
            var x = (0L, 0L);
            foreach (var roll in rolls)
            {
                 var a = XD(!isP1, Mod10(pos1 + roll.v), pos2, score1 + Mod10(pos1 + roll.v), score2);
                 x.Item1 += roll.n * a.Item1;
                 x.Item2 += roll.n * a.Item2;
            }
            return x;
        }
        else
        {
            var x = (0L, 0L);
            foreach (var roll in rolls)
            {
                var a = XD(!isP1, pos1, Mod10(pos2 + roll.v), score1, score2 + Mod10(pos2 + roll.v));
                x.Item1 += roll.n * a.Item1;
                x.Item2 += roll.n * a.Item2;
            }
            return x;
        }
    }
}