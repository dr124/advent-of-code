using System;
using System.IO;
using System.Linq;
using Advent._2019.Intcode;

namespace Advent._2019.Week1
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

            var computer = new Computer(ints);
            computer.Memory[1] = 12;
            computer.Memory[2] = 2;
            computer.Compute();
            Console.WriteLine(computer.Memory[0]);

            //task2
            for (var i = 0; i < 100; i++)
            for (var j = 0; j < 100; j++)
            {
                computer.ResetMemory();
                computer.Memory[1] = i;
                computer.Memory[2] = j;
                computer.Compute();
                if (computer.Memory[0] == 19690720)
                {
                    Console.WriteLine($"found the pair! : {i}, {j} => {i * 100 + j}");
                    return;
                }
            }
        }

       

    }
}