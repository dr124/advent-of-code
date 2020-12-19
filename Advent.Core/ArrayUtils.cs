using System.Numerics;

namespace Advent.Core
{
    //https://stackoverflow.com/questions/1014005/how-to-populate-instantiate-a-c-sharp-array-with-a-single-value

    public static class ArrayUtils
    {
        public static void PopulateSimd<T>(T[] array, T value) where T : struct
        {
            var vector = new Vector<T>(value);
            var i = 0;
            var s = Vector<T>.Count;
            var l = array.Length & ~(s - 1);
            for (; i < l; i += s) vector.CopyTo(array, i);
            for (; i < array.Length; i++) array[i] = value;
        }

        public static T[] Populate<T>(this T[] arr, T value)
        {
            for (var i = 0; i < arr.Length; i++) 
                arr[i] = value;
            return arr;
        }
    }
}
