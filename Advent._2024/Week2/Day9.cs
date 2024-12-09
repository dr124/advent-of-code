using System.Diagnostics;
using System.Text;

namespace Advent._2024.Week2;

public class Day9(string[] input) : IDay
{
    public object Part1()
    {
        var line = input[0];
        var sum = 0L;
        var startNumIdx = -1;
        var endNumIdx = (line.Length - 1) / 2 + 1;
        var posIdx = 0;
        var cnt = 0;
        var cntZeros = 0;
        var cntEnd = 0;

        //"beautiful" linear calculation with no allocation
        while (true)
        {
            // process number
            startNumIdx++;
            cnt = line[startNumIdx * 2] - '0';

            if (startNumIdx == endNumIdx)
            {
                cnt = cntEnd;
            }

            while (cnt > 0)
            {
                sum += startNumIdx * posIdx++;
                cnt--;
            }

            if (startNumIdx == endNumIdx)
            {
                break;
            }

            // process space
            cntZeros = line[startNumIdx * 2 + 1] - '0';
            if (cntEnd > 0)
            {
                //leave
            }
            else
            {
                endNumIdx--;
                cntEnd = line[endNumIdx * 2] - '0';
            }

            while (cntZeros > 0)
            {
                cntZeros--;
                cntEnd--;
                sum += endNumIdx*posIdx++;

                if (cntEnd <= 0 && cntZeros > 0)
                {
                    endNumIdx--;
                    cntEnd = line[endNumIdx * 2] - '0';
                }
            }

        }

        return sum;
    }

    public object Part2()
    {
        var line = input[0];
        List<(int count, int value)> fragments = [];
        for (int i = 0; i < line.Length; i++)
        {
            // numbers
            {
                var cnt = line[i] - '0';
                fragments.Add((cnt, i / 2));
            }

            // zeros
            {
                i++;
                if (i >= line.Length)
                {
                    break;
                }
                var cnt = line[i] - '0';
                if (cnt > 0)
                {
                    fragments.Add((cnt, -1));
                }
            }
        }

        for (int i = fragments.Count - 1; i > 0; i--)
        {
            var toMove = fragments[i];
            if (toMove.value == -1)
            {
                continue;
            }
            var availablePosition = fragments
                .Select((x, idx) => (x.count, x.value, idx))
                .Where(x => x.idx < i)
                .FirstOrDefault(x => x.value == -1 && x.count >= toMove.count);
            if (availablePosition != default)
            {
                var needed = toMove.count;
                var available = availablePosition.count;
                var spaceLeft = available - needed;
                fragments[i] = (toMove.count, -1);
                if (spaceLeft > 0)
                {
                    fragments[availablePosition.idx] = (spaceLeft, -1);
                    fragments.Insert(availablePosition.idx, toMove);
                    i++;
                }
                else
                {
                    fragments[availablePosition.idx] = toMove;
                }

                i -= RemoveLastSpace(fragments);
                i -= MergeSpaces(fragments);
                i++;
            }
        }

        var idx = 0;
        var sum = 0L;
        for (int i = 0; i < fragments.Count; i++)
        {
            for (int j = 0; j < fragments[i].count; j++)
            {
                if (fragments[i].value != -1)
                {
                    sum += fragments[i].value * idx;
                }
                idx++;
            }
        }
        return sum; 
    }

    // 9836645809674 too high

    private int RemoveLastSpace(List<(int count, int value)> fragments)
    {
        var removed = 0;
        for (int i = fragments.Count - 1; i > 0; i--)
        {
            if (fragments[i].value == -1)
            {
                fragments.RemoveAt(i);
                removed++;
            }
            else
            {
                break;
            }
        }

        return removed;
    }

    private int MergeSpaces(List<(int count, int value)> fragments)
    {
        var removed = 0;
        for (int i = 1; i < fragments.Count; i++)
        {
            if (fragments[i].value == -1 && fragments[i - 1].value == -1)
            {
                fragments[i - 1] = (fragments[i - 1].count + fragments[i].count, -1);
                fragments.RemoveAt(i);
                i--;
                removed++;
            }
        }

        return removed;
    }
}