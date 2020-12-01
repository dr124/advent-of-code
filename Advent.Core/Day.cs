using System;
using System.Diagnostics;

namespace Advent.Core
{
    public abstract class Day<TInput, TResult>
    {
        private TResult _resultA;
        private TResult _resultB;
        private TimeSpan _timeA;
        private TimeSpan _timeB;
        protected TInput Input { get; private set; }

        public void Execute()
        {
            try
            {
                Input = ReadData();
            }
            catch (Exception e)
            {
                Console.WriteLine($"# Error parsing data: {e.Message}\n{e}");
                return;
            }

            try
            {
                ProcessTaskA();
            }
            catch (Exception e)
            {
                Console.WriteLine($"# Error running Task A: {e.Message}\n{e}");
                return;
            }

            try
            {
                ProcessTaskB();
            }
            catch (Exception e)
            {
                Console.WriteLine($"# Error running Task B: {e.Message}\n{e}");
                return;
            }

            Console.WriteLine();
            Console.WriteLine("============================");
            Console.WriteLine("All finished");
            Console.WriteLine($"A: {_resultA}");
            Console.WriteLine($"B: {_resultB}");
            Console.WriteLine($"Total time: {(_timeA + _timeB).TotalMilliseconds}ms");
            Console.WriteLine("============================");
        }

        private void ProcessTaskA()
        {
            var timerA = new Stopwatch();
            timerA.Start();
            _resultA = TaskA();
            timerA.Stop();
            _timeA = timerA.Elapsed;

            Console.WriteLine($"Task A: {_resultA}, {_timeA.TotalMilliseconds}ms");
        }

        private void ProcessTaskB()
        {
            var timerB = new Stopwatch();
            timerB.Start();
            _resultB = TaskB();
            timerB.Stop();
            _timeB = timerB.Elapsed;
            Console.WriteLine($"Task B: {_resultB}, {_timeB.TotalMilliseconds}ms");
        }

        protected abstract TInput ReadData();
        protected abstract TResult TaskA();
        protected abstract TResult TaskB();
    }
}