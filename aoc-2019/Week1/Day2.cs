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

            var computer = new IntcodeComputer(ints);
            computer.Memory[1] = 12;
            computer.Memory[2] = 2;
            computer.Process();
            Console.WriteLine(computer.Memory[0]);

            //task2
            for (var i = 0; i < 100; i++)
            for (var j = 0; j < 100; j++)
            {
                computer.ResetMemory();
                computer.Memory[1] = i;
                computer.Memory[2] = j;
                computer.Process();
                if (computer.Memory[0] == 19690720)
                {
                    Console.WriteLine($"found the pair! : {i}, {j} => {i * 100 + j}");
                    return;
                }
            }
        }

       

    }
}