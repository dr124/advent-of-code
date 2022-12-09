using System.Numerics;

namespace Advent.Core.Extensions;
//https://stackoverflow.com/questions/1014005/how-to-populate-instantiate-a-c-sharp-array-with-a-single-value

public static class ArrayUtils
{
    public static void FillSimd<T>(T[] array, T value) where T : struct
    {
        var vector = new Vector<T>(value);
        var i = 0;
        var s = Vector<T>.Count;
        var l = array.Length & ~(s - 1);
        for (; i < l; i += s) vector.CopyTo(array, i);
        for (; i < array.Length; i++) array[i] = value;
    }

    public static T[] Fill<T>(this T[] arr, T value)
    {
        for (var i = 0; i < arr.Length; i++) 
            arr[i] = value;
        return arr;
    }

    public static T[] Populate<T>(this T[] arr, Func<int, T> value)
    {
        for (var i = 0; i < arr.Length; i++)
            arr[i] = value(i);
        return arr;
    }

    public static int ToBitMap(this IEnumerable<bool> arr)
    {
        int i = 0;
        int sum = 0;
        foreach (var a in arr)
        {
            if (a)
                sum += (1 << i);
            i++;
        }

        return sum;
    }
}