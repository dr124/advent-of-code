using Advent.Core_2022.Parsing;
using FluentAssertions;

namespace Advent.Core_2022.Tests.Parsing;

public class InputParserTests_2022d22
{
    private readonly string[] _lines =
    {
        "on x=-5..47,y=-31..22,z=-19..33",
        "on x=-44..5,y=-27..21,z=-14..35",
        "on x=-49..-1,y=-11..42,z=-10..38"
    };

    [Fact]
    public void ParseArray_2022d22()
    {
        // Arrange
        var parser = new InputParser();
        
        // Act
        var result = parser.ParseArray<Row>(_lines, "on x=%d..%d,y=%d..%d,z=%d..%d");

        // Assert
        result.Length.Should().Be(3);
        result[0].Should().BeEquivalentTo(new Row(-5, 47, -31, 22, -19, 33));
        result[1].Should().BeEquivalentTo(new Row(-44, 5, -27, 21, -14, 35));
        result[2].Should().BeEquivalentTo(new Row(-49, -1, -11, 42, -10, 38));
    }

    public record Row(int XMin, int XMax, int YMin, int YMax, int ZMin, int ZMax);
}


public class InputParserTests_2022d19
{
    private readonly string[] _lines =
    {
        "--- scanner 0 ---",
        "0,2",
        "4,1",
        "3,3",
        "",
        "--- scanner 1 ---",
        "-1,-1",
        "-5,0",
        "-2,1",
    };

    [Fact]
    public void ParseArray_2022d22()
    {
        // Arrange
        var parser = new InputParser();

        // Act
        var result = parser.ParseArray<Row>(_lines, "on x=%d..%d,y=%d..%d,z=%d..%d");

        // Assert
        Assert.Fail("not implemented");
    }
    
    public record Row(int scanner, Vector[] positions);
    public record Vector(int X, int Y);
}

