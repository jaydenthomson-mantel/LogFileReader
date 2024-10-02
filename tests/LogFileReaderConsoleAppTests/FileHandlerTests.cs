using FluentAssertions;
using LogFileReaderConsoleApp.Handlers;
using Xunit;

namespace LogFileReaderConsoleAppTests;

public class FileHandlerTests
{
    [Theory]
    [InlineData("file-does-not-exist.log")]
    [InlineData("../../../TestData/BadLogFile.log")]
    public void TryReadFile_ReturnsFalse(string filePath)
    {
        // Act
        var result = FileHandler.TryReadFile(filePath, out var logEntries);

        // Assert
        result.Should().BeFalse();
        logEntries.Should().BeEmpty();
    }

    [Fact]
    public void TryReadFile_FileExists_ReturnsTrue()
    {
        // Arrange
        var filePath = "../../../TestData/SampleLogFile.log";

        // Act
        var result = FileHandler.TryReadFile(filePath, out var logEntries);

        // Assert
        result.Should().BeTrue();
        logEntries.Should().NotBeEmpty();
    }
}
