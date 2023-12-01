using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent.Core_2019_2020;

public static class Utils
{
    public static int Product(this IEnumerable<int> vals) => vals.Aggregate(1, (acc, arr) => acc * arr);
    public static long ProductLong(this IEnumerable<long> vals) => vals.Aggregate(1L, (acc, arr) => acc * arr);


    public static bool IsInRange(this int v, Range r) => v >= r.Start.Value && v <= r.End.Value; 
        
    public static bool IsChar(this int c) => c is >= 'A' and <= 'Z';
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

    /// <summary>
    /// <b>The greatest common</b> factor of a set of whole numbers is the largest positive integer that divides evenly into all numbers with zero remainder.
    /// </summary>
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

    /// <summary>
    /// <b>Least common multiple</b> is the smallest positive integer that is divisible by both a and b.[
    /// </summary>
    public static long lcm(long a, long b)
    {
        return (a / gcf(a, b)) * b;
    }
}