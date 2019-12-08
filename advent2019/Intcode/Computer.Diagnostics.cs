using System.Collections.Generic;
using System.Linq;

namespace advent2019.Intcode
{
    public partial class Computer
    {
        // for diag codes
        public Queue<int> Input { get; set; }
        public List<int> Output { get; set; }

        private int ReadInputValue()
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

        private void WriteOutputValue(int value)
        {
            Log($"outputting: {value}");
            Output.Add(value);
            ProgramOutputted(new OutputEventArgs {OutputValue = value});
        }

        public void ResetMemory()
        {
            Memory = ROM.ToArray();
            Stop = false;
            Pointer = 0;
            Input = new Queue<int>();
            Output = new List<int>();
        }

        public void Log(string s)
        {
            //Console.WriteLine($"{computerName}: {s}");
            //Debug.WriteLine($"{computerName}: {s}");
        }
    }
}