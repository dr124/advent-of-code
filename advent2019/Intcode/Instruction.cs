using System.Collections.Generic;

namespace advent2019.Intcode
{
    public enum Mode
    {
        Pointer = 0,
        Value = 1,
        Relative = 2
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
        SetBase = 9,
        HALT = 99
    }

    public class Instruction
    {
        private readonly Mode _arg1Mode, _arg2Mode, _arg3Mode;
        public readonly long Arg1, Arg2, Arg3;
        public readonly int Arg1Index, Arg2Index, Arg3Index;
        public readonly Operation Op;
        private readonly int _relativeBase;
        private string raw;

        public Instruction(List<long> memory, int i, int relativeBase)
        {
            var str = memory[i].ToString();
            raw = $"{memory[i]}, {memory[i + 1]}, {memory[i + 1]}, {memory[i + 3]}";
            _relativeBase = relativeBase;

            // one digit instr
            Op = (Operation) memory[i];

            

            try
            {
                Op = (Operation) int.Parse(str.Substring(str.Length - 2, 2));
                _arg1Mode = (Mode) int.Parse(str.Substring(str.Length - 3, 1));
                _arg2Mode = (Mode) int.Parse(str.Substring(str.Length - 4, 1));
                _arg3Mode = (Mode) int.Parse(str.Substring(str.Length - 5, 1));
            }
            catch
            {
            }

            try
            {
                Arg1Index = _arg1Mode switch
                {
                    Mode.Pointer => (int)memory[i + 1],
                    Mode.Value => i + 1,
                    Mode.Relative => _relativeBase + (int)memory[i + 1]
                };
                Arg2Index = _arg2Mode switch
                {
                    Mode.Pointer => (int)memory[i + 2],
                    Mode.Value => i + 2,
                    Mode.Relative => _relativeBase + (int)memory[i + 2]
                };
                Arg3Index = _arg3Mode switch
                {
                    Mode.Pointer => (int)memory[i + 3],
                    Mode.Value => i + 3,
                    Mode.Relative => _relativeBase + (int)memory[i + 3]
                };
            }
            catch
            {
            }

            try
            {
                Arg1 = memory[Arg1Index];
                Arg2 = memory[Arg2Index];
                Arg3 = Arg3Index; //param 3 always is index
            }
            catch
            {
            }
        }
    }
}