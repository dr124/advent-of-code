using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Advent._2022.Week2;

public class Day13 : IReadInputDay
{
    public JToken[] _input;
    
    public void ReadData() =>
        _input = File.ReadAllLines("Week2/Day13.txt")
            .Where(x => x != "")
            .Select(x => JsonConvert.DeserializeObject<JToken>(x)!)
            .ToArray();

    public object TaskA() => _input.Chunk(2)
        .Select((x, idx) => (x, idx))
        .Where(x => CompareAnyTokens(x.x[0], x.x[1]) == true)
        .Sum(x => x.idx + 1);

    public object TaskB()
    {
        var _2 = Deserialize("[[2]]");
        var _6 = Deserialize("[[6]]");

        var ordered = _input
            .Append(_2)
            .Append(_6)
            .Order(new TokenComparer())
            .ToList();

        var i2 = ordered.IndexOf(_2);
        var i6 = ordered.IndexOf(_6);

        return (i2+1) * (i6+1);
    }

    private static bool? CompareAnyTokens(JToken a, JToken b) =>
        (a.Type, b.Type) switch
        {
            (JTokenType.Integer, JTokenType.Integer) => CompareIntegers(a, b),
            (JTokenType.Array, JTokenType.Array) => CompareArrays(a, b),
            (JTokenType.Integer, JTokenType.Array) => CompareArrays(Pack(a), b),
            (JTokenType.Array, JTokenType.Integer) => CompareArrays(a, Pack(b)),
            _ => null
        };

    private static bool? CompareIntegers(JToken a, JToken b)
    {
        return a.Value<int>() != b.Value<int>()
            ? a.Value<int>() < b.Value<int>()
            : null;
    }

    private static bool? CompareArrays(JToken a, JToken b)
    {
        for (int iA = 0, iB = 0; ; iA++, iB++)
        {
            if (iA >= a.Children().Count() && iB >= b.Children().Count())
                return null;
            if (iA >= a.Children().Count())
                return true;
            if (iB >= b.Children().Count())
                return false;

            var res = CompareAnyTokens(a[iA], b[iB]);
            if (res != null)
                return res;
        }
    }

    private static JToken Pack(JToken token) => Deserialize($"[{token.Value<int>()}]");
    private static JToken Deserialize(string str) => JsonConvert.DeserializeObject<JToken>(str);
    
    private class TokenComparer : IComparer<JToken>
    {
        public int Compare(JToken x, JToken y) =>
            CompareAnyTokens(x, y) switch
            {
                false => 1,
                null => 0,
                true => -1
            };
    }
}