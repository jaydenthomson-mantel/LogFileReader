using FluentAssertions;
using LogFileReaderConsoleApp.Helpers;
using Xunit;

namespace LogFileReaderConsoleAppTests;

public class ConsoleArgumentsHelperTests
{
    [Fact]
    public void TryGetFilePath_WithValidArgs_ReturnsTrue()
    {
        // Arrange
        var args = new[] { "testfile.log" };

        // Act
        var result = ConsoleArgumentsHelper.TryGetFilePath(args, out var filePath);

        // Assert
        result.Should().BeTrue();
        filePath.Should().Be("testfile.log");
    }

    [Fact]
    public void TryGetFilePath_WithNoArgs_ReturnsFalse()
    {
        // Arrange
        var args = Array.Empty<string>();

        // Act
        var result = ConsoleArgumentsHelper.TryGetFilePath(args, out var filePath);

        // Assert
        result.Should().BeFalse();
        filePath.Should().BeNull();
    }
}