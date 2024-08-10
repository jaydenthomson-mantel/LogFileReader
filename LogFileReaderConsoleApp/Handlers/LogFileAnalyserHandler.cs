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
    public static async Task ReportInsights(IReadOnlyList<ApacheClfLogEntry> logEntries, int top = 3)
    {
        var uniqueIpCountTask = Task.Run(() => LogFileAnalyserService.UniqueIpCount(logEntries));
        var mostVisitedUrlsTask = Task.Run(() => LogFileAnalyserService.MostVisitedUrls(logEntries, top));
        var mostActiveIpsTask = Task.Run(() => LogFileAnalyserService.MostActiveIps(logEntries, top));

        await Task.WhenAll(uniqueIpCountTask, mostVisitedUrlsTask, mostActiveIpsTask);
        
        Console.WriteLine("Log File Insights:");
        Console.WriteLine($"Unique IP Count: {uniqueIpCountTask.Result}");
        
        Console.WriteLine("\nMost Visited URLs:");
        foreach (var url in mostVisitedUrlsTask.Result)
        {
            Console.WriteLine($"URL: {url.Key}, Visits: {url.Value}");
        }
        
        Console.WriteLine("\nMost Active IPs:");
        foreach (var ip in mostActiveIpsTask.Result)
        {
            Console.WriteLine($"IP: {ip.Key}, Requests: {ip.Value}");
        }
    }
}