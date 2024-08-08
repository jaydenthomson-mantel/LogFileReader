using CommonLibrary.Helpers;
using FluentAssertions;
using LogFileReaderLibrary.Services;
using LogFileReaderLibrary.Validators;
using Microsoft.Extensions.Logging;
using Moq;

namespace LogFileReaderLibraryTests.ValidatorTests;

public class HttpRequestLogEntryValidatorTests
{
    [Theory]
    [InlineData("TestData.SampleLogFile.log")]
    public void ValidatorShouldPass(string testResourceName)
    {
        // Arrange
        var testDataStream = StreamHelpers.ReadEmbeddedResourceAsStream(testResourceName);
        
        // Act
        var valid = HttpRequestLogEntryValidator.ValidateOrThrow(testDataStream);
        
        // Assert
        valid.Should().BeTrue();
    }
}