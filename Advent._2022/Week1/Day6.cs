using Advent.Core;

namespace Advent._2022.Week1;

public class Day6 : IReadInputDay
{
    string _input;
    
    public void ReadData()
    {
        _input = File.ReadAllText("Week1/Day6.txt");
    }

    public object TaskA() => FindMarker(_input, 4);
    
    public object TaskB() => FindMarker(_input, 14);

    private static int FindMarker(string s, int n)
    {
        for (int i = n; i < s.Length; i++)
        {
            if (s[(i - n)..i].Distinct().Count() == n)
            {
                return i;
            }
        }

        return -1;
    }
}