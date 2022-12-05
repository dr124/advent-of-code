namespace Advent.Core.Extensions;

public static class StackUtils
{
    public static IEnumerable<T> PopMany<T>(this Stack<T> queue, int count)
    {
        for (var i = 0; i < count; i++)
        {
            yield return queue.Pop();
        }
    }

    public static void PushMany<T>(this Stack<T> queue, IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            queue.Push(item);
        }
    }
}