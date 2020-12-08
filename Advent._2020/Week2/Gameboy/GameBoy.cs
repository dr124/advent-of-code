using System;
using Advent._2020.Week2.Gameboy;

namespace Advent._2020.Week2
{
    public class GameBoy
    {
        private bool _forceStop;
        public Action OnCommandExecuted { get; set; }
        public Action OnProgramFinished { get; set; }
        public Action OnProgramStopped { get; set; }
        public Command[] Commands { get; set; }
        public int Accumulator { get; private set; }
        public int Index { get; private set; }

        private void Execute(Command c)
        {
            switch (c.Instr)
            {
                case Instruction.acc:
                    Accumulator += c.arg;
                    break;
                case Instruction.jmp:
                    Index += c.arg - 1;
                    break;
                case Instruction.nop:
                    break;
            }
        }

        public void Stop()
        {
            _forceStop = true;
        }

        public void Run()
        {
            _forceStop = false;
            Index = 0;

            while (Index < Commands.Length)
            {
                if (_forceStop) break;
                var command = Commands[Index];

                Execute(command);

                OnCommandExecuted?.Invoke();

                Index += 1;
            }

            if (_forceStop)
                OnProgramStopped?.Invoke();
            else
                OnProgramFinished?.Invoke();
        }
    }
}