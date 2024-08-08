using System.Globalization;
using FluentAssertions;
using FluentAssertions.Execution;
using LogFileReaderLibrary.Helpers;

namespace LogFileReaderLibraryTests.HelperTests;

public class HttpRequestLogEntryDeserializerTests
{
    [Fact]
    public void ExpectedLogEntryDeserialization()
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
            logEntry.HttpMethod.Should().Be("GET");
            logEntry.Resource.Should().Be("/intranet-analytics/");
            logEntry.HttpVersion.Should().Be("HTTP/1.1");
            logEntry.StatusCode.Should().Be(200);
            logEntry.ResponseSize.Should().Be(3574);
            logEntry.Referer.Should().Be("-");
            logEntry.UserAgent.Should().Be("Mozilla/5.0 (X11; U; Linux x86_64; fr-FR) AppleWebKit/534.7 (KHTML, like Gecko) Epiphany/2.30.6 Safari/534.7");
        }
    }
}