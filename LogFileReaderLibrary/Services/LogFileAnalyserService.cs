using System.Xml;
using LogFileReaderLibrary.Models;

namespace LogFileReaderLibrary.Services;

/// <summary>
/// This service analyses the contents of a log file.
/// </summary>
public static class LogFileAnalyserService
{
    public static int UniqueIpCount(IEnumerable<HttpRequestLogEntry> logContent)
    {
        var ips = logContent.Select(x => x.IpAddress);
        var distinctIps = ips.Distinct();
        return distinctIps.Count();
    }

    public static IDictionary<string, int> MostVisitedUrls(IEnumerable<HttpRequestLogEntry> logContent, int top)
    {
        var dict = logContent
            .GroupBy(entry => entry.Resource)
            .Select(group => new 
            { 
                Url = group.Key, 
                Count = group.Count(), 
                LatestTimestamp = group.Max(entry => entry.Timestamp),
                LogEntries = group
            })
            .OrderByDescending(x => x.Count)
            .ThenByDescending(x => x.LatestTimestamp)
            .Take(top)
            .ToDictionary(group => group.Url, group => group.Count);
        
        return dict;
    }

    public static IDictionary<string, int> MostActiveIps(IEnumerable<HttpRequestLogEntry> logContent, int top)
    {
        var dict = logContent
            .GroupBy(entry => entry.IpAddress)
            .Select(group => new 
            { 
                IpAddress = group.Key, 
                Count = group.Count(), 
                LatestTimestamp = group.Max(entry => entry.Timestamp),
                LogEntries = group
            })
            .OrderByDescending(x => x.Count)
            .ThenByDescending(x => x.LatestTimestamp)
            .Take(top)
            .ToDictionary(group => group.IpAddress, group => group.Count);
        
        return dict;
    }
}