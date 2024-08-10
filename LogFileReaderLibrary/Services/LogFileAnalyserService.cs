using LogFileReaderLibrary.Models;

namespace LogFileReaderLibrary.Services;

/// <summary>
/// This service analyses the contents of a log file.
/// </summary>
public static class LogFileAnalyserService
{
    /// <summary>
    /// Calculates the number of unique IP addresses in the log content.
    /// </summary>
    /// <param name="logContent">A collection of log entries.</param>
    /// <returns>The count of unique IP addresses.</returns>
    public static int UniqueIpCount(IEnumerable<ApacheClfLogEntry> logContent)
    {
        var ips = logContent.Select(x => x.IpAddress);
        var distinctIps = ips.Distinct();
        return distinctIps.Count();
    }

    /// <summary>
    /// Retrieves the most visited URLs from the log content.
    /// When multiple URLs have the same number of visits, it will favour returning the latest.
    /// </summary>
    /// <param name="logContent">A collection of <see cref="ApacheClfLogEntry"/> objects.</param>
    /// <param name="top">The number of top URLs to return.</param>
    /// <returns>A dictionary where the key is the URL and the value is the visit count.</returns>
    public static IDictionary<string, int> MostVisitedUrls(IEnumerable<ApacheClfLogEntry> logContent, int top)
    {
        var dict = logContent
            .GroupBy(entry => entry.Resource.OriginalString)
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

    /// <summary>
    /// Retrieves the most active IP addresses from the log content.
    /// When multiple IPs have the same number of visits, it will favour returning the latest.
    /// </summary>
    /// <param name="logContent">A collection of <see cref="ApacheClfLogEntry"/> objects.</param>
    /// <param name="top">The number of top IP addresses to return.</param>
    /// <returns>A dictionary where the key is the IP address and the value is the request count.</returns>
    public static IDictionary<string, int> MostActiveIps(IEnumerable<ApacheClfLogEntry> logContent, int top)
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