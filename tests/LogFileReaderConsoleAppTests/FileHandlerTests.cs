using FluentAssertions;
using LogFileReaderConsoleApp.Handlers;
using Xunit;

namespace LogFileReaderConsoleAppTests;

public class FileHandlerTests
{
    [Fact]
    public void TryReadFile_ReturnsFalse_FileDoesNotExist()
    {
        // Arrange
        var filePath = "file-does-not-exist.log";
        var originalOut = Console.Out;
        using var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        try
        {
            // Act
            var result = FileHandler.TryReadFile(filePath, out var logEntries);

            // Assert
            result.Should().BeFalse();
            logEntries.Should().BeEmpty();
            var output = stringWriter.ToString();
            output.Should().Contain($"File not found: {filePath}");
        }
        finally
        {
            // Reset the console output to its original value
            Console.SetOut(originalOut);
        }
    }

    [Fact]
    public void TryReadFile_ReturnsFalse_BadFile()
    {
        // Arrange
        var filePath = "../../../TestData/BadLogFile.log";

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
