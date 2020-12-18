using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Advent.Core;

namespace Advent._2020.Week3
{
    public class Day15 : Day<int[], int>
    {
        protected override int[] ReadData() =>
            File.ReadAllText("Week3/Input15.txt")
                .Split(",", StringSplitOptions.TrimEntries | StringSplitOptions.TrimEntries)
                .Select(int.Parse)
                .ToArray();

        protected override int TaskA() => NthSpokenNumber(2020);

        protected override int TaskB() => NthSpokenNumber(30_000_000);

        // ============================

        private int NthSpokenNumber(int n)
        {
            var dict = new Dictionary<int, (int last, int beforeLast)>();
            int lastSpoken = -1;

            for (int i = 1; i <= Input.Length; i++)
                SpeakNumber(Input[i - 1], i);

            for (int i = Input.Length + 1; i <= n; i++)
            {
                if (dict.TryGetValue(lastSpoken, out var spoken))
                    SpeakNumber(spoken.beforeLast == -1 ? 0 : spoken.last - spoken.beforeLast, i);
                else
                    throw new Exception("lolz");
            }

            void SpeakNumber(int num, int turn)
            {
                lastSpoken = num;
                dict[num] = (turn, dict.ContainsKey(num) ? dict[num].last : -1);
            }

            return lastSpoken;
        }
    }
}
