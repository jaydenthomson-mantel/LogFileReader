using LogFileReaderLibrary.Models;

namespace LogFileReaderLibrary.Services;

/// <summary>
/// This service analyses the contents of a log file.
/// </summary>
public class LogFileAnalyserService
{
    public int UniqueIpCount(List<HttpRequestLogEntry> logContent)
    {
        var ips = logContent.Select(x => x.IpAddress);
        var distinctIps = ips.Distinct();
        return distinctIps.Count();
    }

    public IDictionary<string, int> MostVisitedUrls(List<HttpRequestLogEntry> logContent, int top)
    {
        throw new NotImplementedException();
    }

    public IDictionary<string, int> MostActiveIps(List<HttpRequestLogEntry> logContent, int top)
    {
        throw new NotImplementedException();
    }
}