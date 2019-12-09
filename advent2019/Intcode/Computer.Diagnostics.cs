using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace advent2019.Intcode
{
    public partial class Computer
    {
        // for diag codes
        public Queue<long> Input { get; set; }
        public List<long> Output { get; set; }

        private long ReadInputValue()
        {
            if (Input.Count == 0)
            {
                InputEmpty();
                Log("waiting for input..");
            }

            while (Input.Count == 0)
            {
            }

            var input = Input.Dequeue();
            Log($"got input: {input}");
            return input;
        }

        private void WriteOutputValue(long value)
        {
            Log($"outputting: {value}");
            Output.Add(value);
            ProgramOutputted(new OutputEventArgs {OutputValue = value});
        }

        public void ResetMemory()
        {
            Memory = ROM.Concat(Enumerable.Repeat(0L, ROM.Length+5000)).ToArray();
            Stop = false;
            Pointer = 0;
            Input = new Queue<long>();
            Output = new List<long>();
            RelativeBase = 0;
        }

        public void Log(string s)
        {
            Console.WriteLine($"{computerName}: {s}");
            Debug.WriteLine($"{computerName}: {s}");
        }
    }
}