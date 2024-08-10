using LogFileReaderLibrary.Models;
using LogFileReaderLibrary.Services;

namespace LogFileReaderConsoleApp.Handlers;

/// <summary>
/// Provides methods to analyze log files and report insights.
/// </summary>
public static class LogFileAnalyserHandler
{
    /// <summary>
    /// Analyzes the provided log entries and reports insights such as unique IP count, 
    /// most visited URLs, and most active IPs.
    /// </summary>
    /// <param name="logEntries">A list of log entries to analyze.</param>
    /// <param name="top">The number of top entries to report for most visited URLs and most active IPs. Default is 3.</param>
    public static void ReportInsights(List<ApacheClfLogEntry> logEntries, int top = 3)
    {
        var uniqueIpCount = LogFileAnalyserService.UniqueIpCount(logEntries);
        var mostVisitedUrls = LogFileAnalyserService.MostVisitedUrls(logEntries, top);
        var mostActiveIps = LogFileAnalyserService.MostActiveIps(logEntries, top);
        
        Console.WriteLine("Log File Insights:");
        Console.WriteLine($"Unique IP Count: {uniqueIpCount}");
        
        Console.WriteLine("\nMost Visited URLs:");
        foreach (var url in mostVisitedUrls)
        {
            Console.WriteLine($"URL: {url.Key}, Visits: {url.Value}");
        }
        
        Console.WriteLine("\nMost Active IPs:");
        foreach (var ip in mostActiveIps)
        {
            Console.WriteLine($"IP: {ip.Key}, Requests: {ip.Value}");
        }
    }
}