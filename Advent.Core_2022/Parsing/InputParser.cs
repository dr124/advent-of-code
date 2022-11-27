using System.Numerics;
using System.Reflection;

namespace Advent.Core_2022.Parsing;

public class InputParser
{
    public InputParser()
    {
        
    }

    public T[] ParseArray<T,TDto>(string[] lines, string format, Func<TDto, T> transform)
    {
        var parsed = lines.Select(line =>
        {
            var parser = new ScanFormatted();
            parser.Parse(line, format);
            return parser.Results.ToArray();
        });

        return parsed.Select(CreateType<TDto>).Select(transform).ToArray();
    }

    public T[] ParseArray<T>(string[] lines, string format)
    {
        T NoTransform(T t) => t;
        return ParseArray<T, T>(lines, format, NoTransform);
    }


    private T CreateType<T>(object[] args)
    {
        var c = typeof(T).GetConstructors().FirstOrDefault();

        // fill args with null to desired size
        var argsFilled = new object[c.GetParameters().Length];
        Array.Copy(args, argsFilled, args.Length);

        var pairs = argsFilled.Zip(c.GetParameters())
            .Select(pair => ConvertParameters(pair.First, pair.Second))
            .ToArray();

        return (T)Activator.CreateInstance(typeof(T), args: pairs)!;
    }

    private object ConvertParameters(object arg, ParameterInfo param)
    {
        var type = param.ParameterType;
        if (type.IsEnum)
        {
            return Enum.Parse(type, (string)arg, true);
        }

        return arg;
    }
}