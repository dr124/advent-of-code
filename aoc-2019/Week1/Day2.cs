using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace advent.Week1
{
    public class Day2
    {
        public static void Execute()
        {
            //task 1
            var ints = File.ReadAllText(@"Week1\input2.txt")
                .Split(",",StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();

            //task1 
            ints[1] = 12;
            ints[2] = 2;
            Console.WriteLine(Process(ints)[0]);

            //task2
            for (int i = 0; i < 100; i++)
            for (int j = 0; j < 100; j++)
            {
                ints[1] = i;
                ints[2] = j;
                var result = Process(ints);
                if (result[0] == 19690720)
                    Console.WriteLine($"found the pair! : {i}, {j} => {i * 100 + j}");
            }
        }

        public static int[] Process(int[] ints)
        {
            var list = ints.ToList();
            for (var i = 0; i < list.Count;)
                i = OpCode(list, i);
            return list.ToArray();
        }


        public static int OpCode(List<int> list, int i)
        {
            switch (list[i])
            {
                case 1:
                    list[list[i + 3]] = list[list[i + 1]] + list[list[i + 2]];
                    return i + 3;
                case 2:
                    list[list[i + 3]] = list[list[i + 1]] * list[list[i + 2]];
                    return i+3;
                case 99:
                    return list.Count;
            }
            return i+1;
        }

    }
}