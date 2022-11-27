using Advent.Core_2022.Parsing;

namespace Advent.Core_2022.Tests.Parsing;

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