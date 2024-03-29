﻿using System;
using System.Diagnostics;

namespace Advent.Core_2019_2020;

public abstract class Day<TInput, TResult>
{
    private TResult _resultA;
    private TResult _resultB;
    private TimeSpan _timeA;
    private TimeSpan _timeB;
    private TimeSpan _timeReadData;
    protected TInput Input { get; private set; }

    public void Execute()
    {
        try
        {
            ProcessReadData();
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

    private void ProcessReadData()
    {
        var timer = new Stopwatch();
        timer.Start();

        Input = ReadData();
        timer.Stop();
        _timeReadData = timer.Elapsed;
        Console.WriteLine($"Data read in {_timeReadData.TotalMilliseconds}ms");
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

public abstract class Day : Day<int, int>
{
    protected new abstract void Execute();
    protected override int ReadData()
    {
        this.Execute();
        return -1;
    }

    protected override int TaskA()
    {
        return -1;
    }

    protected override int TaskB()
    {
        return -1;
    }
}