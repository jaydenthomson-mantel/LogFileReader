using CommonLibrary.Helpers;
using FluentAssertions;
using LogFileReaderLibrary.Services;
using LogFileReaderLibrary.Validators;
using Microsoft.Extensions.Logging;
using Moq;

namespace LogFileReaderLibraryTests.ValidatorTests;

public class HttpRequestLogEntryValidatorTests
{
    private readonly Mock<ILogger<HttpRequestLogEntryValidator>> _mockLogger = new();
    
    [Theory]
    [InlineData("TestData.SampleLogFile.log")]
    public void ValidatorShouldPass(string testResourceName)
    {
        // Arrange
        var testDataStream = StreamHelpers.ReadEmbeddedResourceAsStream(testResourceName);
        var validator = new HttpRequestLogEntryValidator(_mockLogger.Object);
        
        // Act
        var valid = validator.Validate(testDataStream);
        
        // Assert
        valid.Should().BeTrue();
    }
}