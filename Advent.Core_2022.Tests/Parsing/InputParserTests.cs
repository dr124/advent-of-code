using Advent.Core_2022.Parsing;
using FluentAssertions;

namespace Advent.Core_2022.Tests.Parsing;

public class InputParserTests
{
    [Fact]
    public void ParseArray_Test1()
    {
        // Arrange
        var lines = new[]
        {
            "1-3 a: abcde",
            "1-3 b: cdefg",
            "2-9 c: ccccccccc",
        };

        var format = "%d-%d %c: %s";

        var parser = new InputParser();

        // Act
        var result = parser.ParseArray<PasswordEntry>(lines, format);

        // Assert
        result.Length.Should().Be(3);

        result[0].Should().BeEquivalentTo(new PasswordEntry(1, 3, 'a', "abcde"));
        result[1].Should().BeEquivalentTo(new PasswordEntry(1, 3, 'b', "cdefg"));
        result[2].Should().BeEquivalentTo(new PasswordEntry(2, 9, 'c', "ccccccccc"));
    }

    private record PasswordEntry(int Min, int Max, char Letter, string Password);
}