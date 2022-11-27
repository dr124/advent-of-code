using Advent.Core_2022.Parsing;
using FluentAssertions;

namespace Advent.Core_2022.Tests.Parsing;

public class InputParserTests_2022d24
{
    private readonly string[] _lines =
    {
        "inp w",
        "mul x 0",
        "add y z",
        "mod z 26",
        "div x y",
        "eql z -5"
    };
    
    [Fact]
    public void ParseArray_2022d24()
    {
        // Arrange
        var parser = new InputParser();
        var transform = (RowDto dto) =>
        {
            int? value = int.TryParse(dto.ThirdArgument, out var i) ? i : null;
            Variable? reference = value == null && Enum.TryParse<Variable>(dto.ThirdArgument, true, out var e) ? e : null;
            return new Row(dto.Operation, dto.Variable, value, reference);
        };
        
        // Act
        var result = parser.ParseArray(_lines, "%s %s %s", transform);

        // Assert
        result.Length.Should().Be(6);
        result[0].Should().BeEquivalentTo(new Row(Operation.Inp, Variable.W, null, null));
        result[1].Should().BeEquivalentTo(new Row(Operation.Mul, Variable.X, 0, null));
        result[2].Should().BeEquivalentTo(new Row(Operation.Add, Variable.Y, null, Variable.Z));
        result[3].Should().BeEquivalentTo(new Row(Operation.Mod, Variable.Z, 26, null));
        result[4].Should().BeEquivalentTo(new Row(Operation.Div, Variable.X, null, Variable.Y));
        result[5].Should().BeEquivalentTo(new Row(Operation.Eql, Variable.Z, -5, null));
    }

    public record RowDto(Operation Operation, Variable Variable, string ThirdArgument);
    public record Row(Operation Operation, Variable Variable, int? Value, Variable? Reference);
    public enum Operation { Inp, Mul, Add, Mod, Div, Eql }
    public enum Variable { W, X, Y, Z };
}