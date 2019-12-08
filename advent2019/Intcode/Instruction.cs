using System.Collections.Generic;

namespace advent2019.Intcode
{
    public enum Mode
    {
        Pointer = 0,
        Value = 1
    }
    public enum Operation
    {
        Add = 1,
        Multiply = 2,
        ReadInput = 3,
        WriteOutput = 4,
        JumpTrue = 5,
        JumpZero = 6,
        LessThan = 7,
        AreEqual = 8,
        HALT = 99
    }

    public class Instruction
    {
        public readonly Operation Op;
        public readonly int Arg1, Arg2, Arg3;
        private readonly Mode _arg1Mode, _arg2Mode, _arg3Mode;
        private string raw;

        public Instruction(List<int> memory, int i)
        {
            var str = memory[i].ToString();
            raw = str;

            // one digit instr
            Op = (Operation)memory[i];

            try
            {
                Op = (Operation)int.Parse(str.Substring(str.Length - 2, 2));
                _arg1Mode = (Mode)int.Parse(str.Substring(str.Length - 3, 1));
                _arg2Mode = (Mode)int.Parse(str.Substring(str.Length - 4, 1));
                _arg3Mode = (Mode)int.Parse(str.Substring(str.Length - 5, 1));
            }
            catch
            {
            }

            try
            {
                Arg1 = _arg1Mode == Mode.Value ? memory[i + 1] : memory[memory[i + 1]];
                Arg2 = _arg2Mode == Mode.Value ? memory[i + 2] : memory[memory[i + 2]];
                Arg3 = memory[i + 3]; //param 3 always position mode
            }
            catch
            {
            }
        }


    }
}
