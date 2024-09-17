using CommonLibrary.Helpers;
using FluentAssertions;
using LogFileReaderLibrary.Helpers;
using LogFileReaderLibrary.Services;

namespace LogFileReaderLibraryTests.ServiceTests;

public class LogFileAnalyserServiceTests
{
    [Theory]
    [InlineData("TestData.SampleLogFile.log", 11)]
    [InlineData("TestData.EmptyLogFile.log", 0)]
    public void UniqueIpCountReturnsCorrectCount(string testResourceName, int expectedCount)
    {
        // Arrange
        var testDataStream = StreamHelpers.ReadEmbeddedResourceAsStream(testResourceName);
        var testList = HttpRequestLogEntryDeserializer.DeserializeApacheClfList(testDataStream);
        
        // Act
        var actualCount = LogFileAnalyserService.UniqueIpCount(testList);

        // Assert
        actualCount.Should().Be(expectedCount);
    }
    
    [Theory]
    [InlineData("TestData.SampleLogFile.log", 3, "/docs/manage-websites/", "/temp-redirect", "/moved-permanently")]
    [InlineData("TestData.EmptyLogFile.log", 3)]
    public void MostVisitedUrlsReturnsCorrectUrls(string testResourceName, int top, params string[] expectedTopUrls)
    {
        // Arrange
        var testDataStream = StreamHelpers.ReadEmbeddedResourceAsStream(testResourceName);
        var testList = HttpRequestLogEntryDeserializer.DeserializeApacheClfList(testDataStream);
        
        // Act
        var actualTopUrls = LogFileAnalyserService.MostVisitedUrls(testList, top).Keys.ToArray();

        // Assert
        actualTopUrls.Should().Equal(expectedTopUrls);
    }
    
    [Theory]
    [InlineData("TestData.SampleLogFile.log", 3, "168.41.191.40", "50.112.00.11", "177.71.128.21")]
    [InlineData("TestData.EmptyLogFile.log", 3)]
    public void MostActiveIpsReturnsCorrectIps(string testResourceName, int top, params string[] expectedTopIps)
    {
        // Arrange
        var testDataStream = StreamHelpers.ReadEmbeddedResourceAsStream(testResourceName);
        var testList = HttpRequestLogEntryDeserializer.DeserializeApacheClfList(testDataStream);
        
        // Act
        var actualTopUrls = LogFileAnalyserService.MostActiveIps(testList, top).Keys.ToArray();

        // Assert
        actualTopUrls.Should().Equal(expectedTopIps);
    }
}