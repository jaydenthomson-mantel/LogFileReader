using FluentAssertions;
using LogFileReaderConsoleApp.Helpers;
using LogFileReaderLibrary.Models.Exceptions;
using Xunit;

namespace LogFileReaderConsoleAppTests;

public class ConsoleErrorMessagesHelperTests
{
    [Fact]
    public void GetBadApacheClfFileExceptionMessage_ReturnsExpectedMessage()
    {
        // Arrange
        var ex = new BadApacheClfFileException(
        [
            new ApacheClfLogValidationException("Invalid Apache CLF log line",
            [
                new("Inner exception message")
            ])
        ]);

        var expectedMessage = "One or more exceptions occured attempting to deserialize an Apache clf file. See the inner exceptions for more details.\n" +
            "\tOne or more properties in the log line 'Invalid Apache CLF log line' were invalid.\n" +
            "\t\tInner exception message\n";

        // Act
        var result = ConsoleErrorMessagesHelper.GetBadApacheClfFileExceptionMessage(ex);

        // Assert
        result.Should().Be(expectedMessage);
    }
}
