using System;
using System.Linq;
using System.Runtime.CompilerServices;

#pragma warning disable 8509

namespace advent2019.Intcode
{
    public partial class Computer
    {
        private readonly int[] ROM; // read only memory
        private Instruction Instr; // current instruction
        public int[] Memory; // program memory
        private int Pointer; // instruction pointer
        private bool Stop;
        private string computerName;

        public Computer(int[] Instructions, [CallerMemberName] string name = "")
        {
            computerName = name;
            ROM = Instructions.ToArray();
            ResetMemory();
        }

        public void Compute()
        {
            while (!Stop) ProcessInstruction();
        }

        private void ProcessInstruction()
        {
            Instr = new Instruction(Memory.ToList(), Pointer);

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
                case Operation.HALT:
                    ProgramFinished();
                    break;
                default:
                    throw new Exception("witam, error, pozdrawiam");
            }
        }
    }
}