using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace advent
{
    public class IntcodeComputer
    {
        private List<int> ROM { get; } // read only memory
        public List<int> Memory { get; set; } // program memory

        public IntcodeComputer(int[] instructions)
        {
            ROM = instructions.ToList();
            Memory = ROM.ToList();
        }

        public void Process()
        {   
            for (var i = 0; i < Memory.Count;)
                i = OpCode(i);
        }

        public void ResetMemory()
        {
            Memory = ROM.ToList();
        }

        public int DiagCode { get; set; }

        public int MathCodes(int i)
        {
            var instr = new Instruction(Memory, i);
            switch (instr.Instr)
            {
                case 1:
                    Memory[instr.Param3Value] = instr.Param2Value + instr.Param1Value;
                    break;
                case 2:
                    Memory[instr.Param3Value] = instr.Param2Value * instr.Param1Value;
                    break;
            }

            return i + 4;
        }

        public int DiagCodes(int i)
        {
            var instr = new Instruction(Memory, i);
            switch (instr.Instr)
            {
                case 3: // input a value
                    Console.WriteLine("INPUT CODE");
                    Memory[Memory[i + 1]] = DiagCode;
                    break;
                case 4: // output a value
                    var xd = Memory[Memory[i + 1]];
                    Console.WriteLine($"{xd}");
                    Debug.WriteLine($"{xd}");
                    break;
            }

            return i + 2;
        }

        public int JumpCodes(int i)
        {
            var instr = new Instruction(Memory, i);
            switch (instr.Instr)
            {
                case 5: // jump if true 
                    if (instr.Param1Value != 0)
                        return instr.Param2Value;
                    break;
                case 6: // jump is zero
                    if (instr.Param1Value == 0)
                        return instr.Param2Value;
                    break;
            }

            return i + 3;
        }

        public int IfCodes(int i)
        {
            var instr = new Instruction(Memory, i);
            switch (instr.Instr)
            {
                case 7: // less than
                    Memory[instr.Param3Value] = instr.Param1Value < instr.Param2Value ? 1 : 0;
                    break;
                case 8: // equals
                    Memory[instr.Param3Value] = instr.Param1Value == instr.Param2Value ? 1 : 0;
                    break;
            }

            return i + 4;
        }

        public int OpCode(int i)
        {
            var instr = new Instruction(Memory, i);
            switch (instr.Instr)
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
                    return Memory.Count; // program exit
                default:
                    throw new Exception("witam, error, pozdrawiam");
            }
        }

        public class Instruction
        {
            public int Instr;
            public int Param1Value, Param2Value, Param3Value;
            public int? Param1Mode, Param2Mode, Param3Mode;

            public Instruction(List<int> list, int i)
            {
                var str = list[i].ToString();

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
