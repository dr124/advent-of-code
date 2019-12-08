using System;
using System.Collections.Generic;
using System.Linq;

#pragma warning disable 8509

namespace advent
{
    public class IntcodeComputer
    {
        private readonly int[] ROM;         // read only memory
        public int[] Memory;                // program memory
        private Instruction Instr;          // current instruction
        private int Pointer;                // instruction pointer
        private bool Stop;

        // for diag codes
        public List<int> Input;
        public List<int> Output;
        private int InputPointer;
        private int GetInputValue() => InputPointer < Input.Count ? Input[InputPointer] : Input.Last();

        private int OutputValue{ get => Output.Last(); set => Output.Add(value); }

        public IntcodeComputer(int[] Instructions)
        {
            ROM = Instructions.ToArray();
            ResetMemory();
        }

        public void Compute()
        {
            while (!Stop) ProcessInstruction();
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

        private void MathCodes()
        {
            Memory[Instr.Param3Value] = Instr.Instr switch
            {
                1 => (Instr.Param2Value + Instr.Param1Value),
                2 => (Instr.Param2Value * Instr.Param1Value),
            };

            Pointer += 4;
        }

        private void DiagCodes()
        {
            switch (Instr.Instr)
            {
                case 3:
                    var xd = GetInputValue();
                    Memory[Memory[Pointer + 1]] = xd;
                    InputPointer += 1;
                    break;
                case 4:
                    OutputValue = Memory[Memory[Pointer + 1]];
                    break;
            }

            Pointer += 2;
        }

        private void JumpCodes()
        {
            switch (Instr.Instr)
            {
                case 5 when Instr.Param1Value != 0:
                case 6 when Instr.Param1Value == 0:
                    Pointer = Instr.Param2Value;
                    break;
                default:
                    Pointer += 3;
                    break;
            }
        }

        private void IfCodes()
        {
            Memory[Instr.Param3Value] = Instr.Instr switch
            {
                7 => (Instr.Param1Value < Instr.Param2Value ? 1 : 0),
                8 => (Instr.Param1Value == Instr.Param2Value ? 1 : 0),
            };

            Pointer += 4;
        }

        private void ProcessInstruction()
        {
            if (Pointer >= Memory.Length)
            {
                Stop = true;
                return;
            }

            Instr = new Instruction(Memory.ToList(), Pointer);

            switch (Instr.Instr)
            {
                case 1:
                case 2:
                    MathCodes();
                    break;
                case 3:
                case 4:
                    DiagCodes();
                    break;
                case 5:
                case 6:
                    JumpCodes();
                    break;
                case 7:
                case 8:
                    IfCodes();
                    break;
                case 99:
                    Pointer = Memory.Length; // program exit
                    break;
                default:
                    throw new Exception("witam, error, pozdrawiam");
            }
        }

        private class Instruction
        {
            public readonly int Instr;
            private readonly int Param1Mode, Param2Mode, Param3Mode;
            public readonly int Param1Value, Param2Value, Param3Value;
            private string raw;

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
                catch
                {
                }

                try
                {
                    Param1Value = Param1Mode == 1 ? list[i + 1] : list[list[i + 1]];
                    Param2Value = Param2Mode == 1 ? list[i + 2] : list[list[i + 2]];
                    Param3Value = list[i + 3]; //param 3 always position mode
                }
                catch
                {
                }
            }
        }
    }
}