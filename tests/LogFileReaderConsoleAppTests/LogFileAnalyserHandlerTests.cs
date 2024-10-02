using Xunit;
using LogFileReaderConsoleApp.Handlers;
using LogFileReaderLibrary.Models;
using FluentAssertions;
using System.Net;

namespace LogFileReaderConsoleAppTests
{
    public class LogFileAnalyserHandlerTestsTest
    {
        [Fact]
        public async Task ReportInsights_ShouldReturnExpectedResult()
        {
            // Arrange
            var logEntries = new List<LogEntry>
            {
                new() {
                    IpAddress = "192.168.1.1",
                    Identd = "-",
                    UserId = "user1",
                    Timestamp = DateTimeOffset.Now,
                    HttpMethod = HttpMethod.Get,
                    HttpVersion = "HTTP/1.1",
                    Resource = new Uri("http://example.com/resource"),
                    StatusCode = HttpStatusCode.OK,
                    ResponseSize = 1234,
                    Referer = "http://example.com",
                    UserAgent = "Mozilla/5.0"
                }
            }; 

            var expected = "Log File Insights:\n" +
                           "Unique IP Count: 1\n\n" +
                           "Most Visited URLs:\n" +
                           "URL: http://example.com/resource, Visits: 1\n\n" +
                           "Most Active IPs:\n" +
                           "IP: 192.168.1.1, Requests: 1\n";
            

            // Act
            var result = await LogFileAnalyserHandler.ReportInsights(logEntries);

            // Assert
            result.Should().Be(expected);
        }
    }
}