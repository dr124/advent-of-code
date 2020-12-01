using System;
using System.Linq;
using System.Runtime.CompilerServices;

#pragma warning disable 8509

namespace Advent._2019.Intcode
{
    public partial class Computer
    {
        private readonly long[] ROM; // read only memory
        private string computerName;
        private Instruction Instr; // current instruction
        public long[] Memory; // program memory
        private int Pointer; // instruction pointer
        private int RelativeBase;
        public bool Stop;

        public Computer(long[] instructions, [CallerMemberName] string name = "")
        {
            computerName = name;
            ROM = instructions.ToArray();
            ResetMemory();
        }

        public Computer(int[] instructions, [CallerMemberName] string name = "")
            : this(instructions.Select(x => (long) x).ToArray(), name)
        {
        }

        public void Compute()
        {
            while (!Stop) ProcessInstruction();
        }

        private void ProcessInstruction()
        {
            Instr = new Instruction(Memory.ToList(), Pointer, RelativeBase);
            Log($"i{Pointer},b{RelativeBase} - OP: {Instr.Op.ToString()}, I: {Memory[Pointer]}");
            switch (Instr.Op)
            {
                case Operation.Add:
                case Operation.Multiply:
                    MathCodes();
                    break;
                case Operation.ReadInput:
                case Operation.WriteOutput:
                    DiagCodes();
                    break;
                case Operation.JumpTrue:
                case Operation.JumpZero:
                    JumpCodes();
                    break;
                case Operation.AreEqual:
                case Operation.LessThan:
                    IfCodes();
                    break;
                case Operation.SetBase:
                    SetBase();
                    break;
                case Operation.HALT:
                    ProgramFinished();
                    break;
                default:
                    throw new Exception("witam, error, pozdrawiam");
            }
        }
    }
}