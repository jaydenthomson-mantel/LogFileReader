using CommonLibrary.Helpers;
using FluentAssertions;
using LogFileReaderLibrary.Helpers;
using LogFileReaderLibrary.Services;

namespace LogFileReaderLibraryTests.ServiceTests;

public class LogFileAnalyserServiceTests
{
    [Theory]
    [InlineData("TestData.SampleLogFile.log", 11)]
    public void UniqueIpCountReturnsCorrectCount(string testResourceName, int expectedCount)
    {
        // Arrange
        var testDataStream = StreamHelpers.ReadEmbeddedResourceAsStream(testResourceName);
        var testList = HttpRequestLogEntryDeserializer.DeserializeApacheClfList(testDataStream);
        var analyser = new LogFileAnalyserService();
        
        // Act
        var actualCount = analyser.UniqueIpCount(testList);

        // Assert
        actualCount.Should().Be(expectedCount);
    }
    
    [Theory]
    [InlineData("TestData.SampleLogFile.log", 3, "/docs/manage-websites/", "/faq/", "/intranet-analytics/")]
    public void MostVisitedUrlsReturnsCorrectUrls(string testResourceName, int top, params string[] expectedTopUrls)
    {
        // Arrange
        var testDataStream = StreamHelpers.ReadEmbeddedResourceAsStream(testResourceName);
        var testList = HttpRequestLogEntryDeserializer.DeserializeApacheClfList(testDataStream);
        var analyser = new LogFileAnalyserService();
        
        // Act
        var actualTopUrls = analyser.MostVisitedUrls(testList, top).Keys.ToArray();

        // Assert
        actualTopUrls.Should().Equal(expectedTopUrls);
    }
    
    [Theory]
    [InlineData("TestData.SampleLogFile.log", 3, "168.41.191.40", "177.71.128.21", "50.112.00.11")]
    public void MostActiveIpsReturnsCorrectIps(string testResourceName, int top, params string[] expectedTopIps)
    {
        // Arrange
        var testDataStream = StreamHelpers.ReadEmbeddedResourceAsStream(testResourceName);
        var testList = HttpRequestLogEntryDeserializer.DeserializeApacheClfList(testDataStream);
        var analyser = new LogFileAnalyserService();
        
        // Act
        var actualTopUrls = analyser.MostActiveIps(testList, top).Keys.ToArray();

        // Assert
        actualTopUrls.Should().Equal(expectedTopIps);
    }
}