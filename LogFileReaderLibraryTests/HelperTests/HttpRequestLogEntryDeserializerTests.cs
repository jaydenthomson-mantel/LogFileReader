using System.Globalization;
using System.Net;
using CommonLibrary.Helpers;
using FluentAssertions;
using FluentAssertions.Execution;
using LogFileReaderLibrary.Helpers;
using LogFileReaderLibrary.Models.Exceptions;

namespace LogFileReaderLibraryTests.HelperTests;

public class HttpRequestLogEntryDeserializerTests
{
    [Theory]
    [InlineData("TestData.SampleLogFile.log", 23)]
    [InlineData("TestData.EmptyLogFile.log", 0)]
    public void SuccessfulLogEntryListDeserialization(string testResourceName, int size)
    {
        // Arrange
        var testDataStream = StreamHelpers.ReadEmbeddedResourceAsStream(testResourceName);
        
        // Act
        var list = HttpRequestLogEntryDeserializer.DeserializeApacheClfList(testDataStream);
        
        // Assert
        list.Count.Should().Be(size);
    }
    
    [Theory]
    [InlineData("TestData.BadLogFile.log", 3)]
    public void FailLogEntryListDeserialization(string testResourceName, int amountOfBadLines)
    {
        // Arrange
        var testDataStream = StreamHelpers.ReadEmbeddedResourceAsStream(testResourceName);
        
        // Act & Assert
        var exception = Assert.Throws<BadApacheClfFileException>(() => HttpRequestLogEntryDeserializer.DeserializeApacheClfList(testDataStream));
        exception.InnerExceptions.Count.Should().Be(amountOfBadLines);
    }
    
    [Fact]
    public void SuccessfulLogEntryDeserializationMapping()
    {
        // Arrange
        const string logEntryRaw = "177.71.128.21 - - [10/Jul/2018:22:21:28 +0200] \"GET /intranet-analytics/ HTTP/1.1\" 200 3574 \"-\" \"Mozilla/5.0 (X11; U; Linux x86_64; fr-FR) AppleWebKit/534.7 (KHTML, like Gecko) Epiphany/2.30.6 Safari/534.7\"";
        
        // Act
        var logEntry = HttpRequestLogEntryDeserializer.DeserializeApacheClf(logEntryRaw);
        
        // Assert
        using (new AssertionScope())
        {
            logEntry.IpAddress.Should().Be("177.71.128.21");
            logEntry.Identd.Should().Be("-");
            logEntry.UserId.Should().Be("-");
            logEntry.Timestamp.Should().Be(DateTimeOffset.ParseExact("10/Jul/2018:22:21:28 +0200", "dd/MMM/yyyy:HH:mm:ss zzz", CultureInfo.InvariantCulture));
            logEntry.HttpMethod.Should().Be(HttpMethod.Get);
            logEntry.Resource.OriginalString.Should().Be("/intranet-analytics/");
            logEntry.HttpVersion.Should().Be("HTTP/1.1");
            logEntry.StatusCode.Should().Be(HttpStatusCode.OK);
            logEntry.ResponseSize.Should().Be(3574);
            logEntry.Referer.Should().Be("-");
            logEntry.UserAgent.Should().Be("Mozilla/5.0 (X11; U; Linux x86_64; fr-FR) AppleWebKit/534.7 (KHTML, like Gecko) Epiphany/2.30.6 Safari/534.7");
        }
    }

    [Theory]
    [InlineData("177.71.128.21 - - [10/Jul/2018:22:21:28 +0200] \"GET /intranet-analytics/ HTTP/1.1\" 200 3574 \"-\" \"Mozilla/5.0 (X11; U; Linux x86_64; fr-FR) AppleWebKit/534.7 (KHTML, like Gecko) Epiphany/2.30.6 Safari/534.7\"")]
    [InlineData("72.44.32.10 - - [09/Jul/2018:15:48:07 +0200] \"GET / HTTP/1.1\" 200 3574 \"-\" \"Mozilla/5.0 (compatible; MSIE 10.6; Windows NT 6.1; Trident/5.0; InfoPath.2; SLCC1; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729; .NET CLR 2.0.50727) 3gpp-gba UNTRUSTED/1.0\" junk extra")]
    [InlineData("168.41.191.9 - - [09/Jul/2018:22:56:45 +0200] \"GET /docs/ HTTP/1.1\" 200 3574 \"-\" \"Mozilla/5.0 (X11; Linux i686; rv:6.0) Gecko/20100101 Firefox/6.0\" 456 789")]
    public void SuccessfulLogEntryDeserialization(string logEntryRaw)
    {
        // Act
        var logEntry = HttpRequestLogEntryDeserializer.DeserializeApacheClf(logEntryRaw);
        
        // Assert
        using (new AssertionScope())
        {
            logEntry.Should().NotBeNull();
            logEntry.IpAddress.Should().NotBeNull();
            logEntry.Identd.Should().NotBeNull();
            logEntry.UserId.Should().NotBeNull();
            logEntry.Timestamp.Should().NotBe(default);
            logEntry.HttpMethod.Should().NotBeNull();
            logEntry.Resource.Should().NotBeNull();
            logEntry.HttpVersion.Should().NotBeNull();
            logEntry.StatusCode.Should().NotBe(default);
            logEntry.ResponseSize.Should().NotBe(default);
            logEntry.Referer.Should().NotBeNull();
            logEntry.UserAgent.Should().NotBeNull();
        }
    }

    [Theory]
    [InlineData("This is not a log.")]
    public void FailLogEntryDeserializationPatternMatching(string logEntryRaw)
    {
        // Act and Assert
        var exception = Assert.Throws<FormatException>(() => HttpRequestLogEntryDeserializer.DeserializeApacheClf(logEntryRaw));
    }
    
    [Theory]
    [InlineData("177.71.128.0.0 - - [Jul/10/2018:22:21:28 +0200] \"GT http:// HTTP/1.1\" 020 03574 \"-\" \"Mozilla/5.0 (X11; U; Linux x86_64; fr-FR) AppleWebKit/534.7 (KHTML, like Gecko) Epiphany/2.30.6 Safari/534.7\"", 4)]
    public void FailLogEntryDeserializationMultipleFormatExceptions(string logEntryRaw, int numberOfExceptions)
    {
        // Act and Assert
        var exception = Assert.Throws<ApacheClfLogValidationException>(() => HttpRequestLogEntryDeserializer.DeserializeApacheClf(logEntryRaw));
        exception.InnerExceptions.Count.Should().Be(numberOfExceptions);
    }
}