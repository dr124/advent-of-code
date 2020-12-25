using System.IO;
using System.Linq;
using Advent.Core;

namespace Advent._2020.Week4
{
    public class Day25 : Day<(int card, int door), int>
    {
        private const int DIVIDER = 20201227;
        private const int SUBJECT = 7;

        protected override (int card, int door) ReadData()
        {
            var f = File.ReadAllLines("Week4/Input25.txt")
                .Select(int.Parse)
                .ToArray();
            return (f[0], f[1]);
        }

        protected override int TaskA()
        {
            var (v, i) = Transform(SUBJECT, int.MaxValue, Input);

            return Transform(v == Input.card ? Input.door : Input.card, i).v;
        }

        private (int v, int i) Transform(int subject, int loopSize, (int card, int door)? search = null)
        {
            var v = 1L;
            var i = 0;

            do v = (v * subject) % DIVIDER;
            while (++i < loopSize && v != search?.card && v != search?.door);

            return ((int)v, i);
        }

        //not used in day25
        protected override int TaskB() => -1;
    }
}