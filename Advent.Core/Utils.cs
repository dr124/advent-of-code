using System;
using System.Collections.Generic;

namespace Advent.Core
{
    public static class Utils
    {
        public static bool IsChar(this int c) => c >= 'A' && c <= 'Z';
        public static bool IsChar(this char c) => IsChar((int)c);
        
        public static Vec2 Vec2(this (int x, int y) v) => new Vec2(v);
        //public static Vec3 Vec3(this (int x, int y, int z) v) => new Vec3(v);

        public static void EnqueueMany<T>(this Queue<T> queue, IEnumerable<T> value)
        {
            foreach (var v in value) 
                queue.Enqueue(v);
        }

        public static IEnumerable<int> To(this Range r)
        {
            for (var i = r.Start.Value; i < r.End.Value; i++)
                yield return i;
        }
        public static long gcf(long a, long b)
        {
            while (b != 0)
            {
                var temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        public static long lcm(long a, long b)
        {
            return (a / gcf(a, b)) * b;
        }
    }
}
