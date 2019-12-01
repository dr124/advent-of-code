using System;
using System.Collections.Generic;
using System.Linq;

namespace advent.Week1
{
    public static class Day4
    {
        //input 
        public const int start = 206938;
        public const int end = 679128;

        public static void Execute()
        {
            int taskA = 0, taskB = 0;

            for (var i = start; i <= end; i++)
            {
                var str = i.ToString();
                if (!IsNeverDecreasing(str))
                    continue;

                if (AreTwoAdjacent(str))
                    taskA++;

                if (AreExactlyTwoAdjacent(str))
                    taskB++;
            }

            Console.WriteLine($"finished, A: {taskA}, B: {taskB}");
        }

        public static bool IsNeverDecreasing(string str)
        {
            for (var i = 1; i < str.Length; i++)
                if (str[i - 1] > str[i])
                    return false;
            return true;
        }

        public static bool AreTwoAdjacent(string str)
        {
            for (var i = 1; i < str.Length; i++)
                if (str[i - 1] == str[i])
                    return true;
            return false;
        }

        public static bool AreExactlyTwoAdjacent(string str)
        {
            return str.GroupBy(x => x)
                .Any(x => x.Count() == 2);
        }
    }
}