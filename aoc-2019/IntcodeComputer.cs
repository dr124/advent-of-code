using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace advent
{
    public class IntcodeComputer
    {
        public int[] Memory { get; set; } // program memory

        // for diag codes
        public List<int> Input;
        public List<int> Output;
        private int InputPointer;
        private int InputValue => Input[InputPointer++];
        private int OutputValue
        {
            set => Output.Add(value);
        }

        private readonly int[] ROM; // read only memory
        private Instruction Instr;  // current instruction
        private int Pointer;        // instruction pointer
        private bool Stop;

        public IntcodeComputer(int[] Instructions)
        {
            ROM = Instructions.ToArray();
            ResetMemory();
        }

        public void Compute()
        {
            while (!Stop)
            {
                Pointer = OpCode(Pointer);
            }
        }

        public void ResetMemory()
        {
            Memory = ROM.ToArray();
            Stop = false;
            Pointer = 0;
            Output = new List<int>();
            Input = new List<int>();
            InputPointer = 0;
        }

        public int MathCodes(int i)
        {
            switch (Instr.Instr)
            {
                case 1:
                    Memory[Instr.Param3Value] = Instr.Param2Value + Instr.Param1Value;
                    break;
                case 2:
                    Memory[Instr.Param3Value] = Instr.Param2Value * Instr.Param1Value;
                    break;
            }

            return i + 4;
        }

        public int DiagCodes(int i)
        {
            switch (Instr.Instr)
            {
                case 3: // input a value
                    Memory[Memory[i + 1]] = InputValue; 
                    break;
                case 4: // output a value
                    OutputValue = Memory[Memory[i + 1]];
                    break;
            }

            return i + 2;
        }

        public int JumpCodes(int i)
        {
            switch (Instr.Instr)
            {
                case 5: // jump if true 
                    if (Instr.Param1Value != 0)
                        return Instr.Param2Value;
                    break;
                case 6: // jump is zero
                    if (Instr.Param1Value == 0)
                        return Instr.Param2Value;
                    break;
            }

            return i + 3;
        }

        public int IfCodes(int i)
        {
            switch (Instr.Instr)
            {
                case 7: // less than
                    Memory[Instr.Param3Value] = Instr.Param1Value < Instr.Param2Value ? 1 : 0;
                    break;
                case 8: // equals
                    Memory[Instr.Param3Value] = Instr.Param1Value == Instr.Param2Value ? 1 : 0;
                    break;
            }

            return i + 4;
        }

        public int OpCode(int i)
        {
            if (Pointer >= Memory.Length)
            {
                Stop = true;
                return 0;
            }

            Instr = new Instruction(Memory.ToList(), Pointer);

            switch (Instr.Instr)
            {
                case 1:
                case 2:
                    return MathCodes(i);
                case 3:
                case 4:
                    return DiagCodes(i);
                case 5:
                case 6:
                    return JumpCodes(i);
                case 7:
                case 8:
                    return IfCodes(i);
                case 99:
                    return Memory.Length; // program exit
                default:
                    throw new Exception("witam, error, pozdrawiam");
            }
        }

        public class Instruction
        {
            private string raw;
            public int Instr;
            public int Param1Value, Param2Value, Param3Value;
            public int Param1Mode, Param2Mode, Param3Mode;

            public Instruction(List<int> list, int i)
            {
                var str = list[i].ToString();
                raw = str;
                // one digit instr
                Instr = list[i];

                try
                {
                    Instr = int.Parse(str.Substring(str.Length - 2, 2));
                    Param1Mode = int.Parse(str.Substring(str.Length - 3, 1));
                    Param2Mode = int.Parse(str.Substring(str.Length - 4, 1));
                    Param3Mode = int.Parse(str.Substring(str.Length - 5, 1));
                }
                catch { }

                try
                {
                    Param1Value = Param1Mode == 1 ? list[i + 1] : list[list[i + 1]];
                    Param2Value = Param2Mode == 1 ? list[i + 2] : list[list[i + 2]];
                    Param3Value = list[i + 3]; //param 3 always position mode
                }
                catch { }
            }
        }
    }
}
