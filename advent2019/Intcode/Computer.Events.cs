using System;

namespace advent2019.Intcode
{
    public partial class Computer
    {
        public event EventHandler<OutputEventArgs> OnProgramOutput;
        public event EventHandler OnProgramFinish;
        public event EventHandler OnInputEmpty;

        private void ProgramOutputted(OutputEventArgs e)
        {
            OnProgramOutput?.Invoke(this, e);
        }

        private void ProgramFinished()
        {
            Stop = true;
            OnProgramFinish?.Invoke(this, new EventArgs());
        }

        private void InputEmpty()
        {
            OnInputEmpty?.Invoke(this, new EventArgs());
        }

        public class OutputEventArgs : EventArgs
        {
            public int OutputValue { get; set; }
        }
    }
}