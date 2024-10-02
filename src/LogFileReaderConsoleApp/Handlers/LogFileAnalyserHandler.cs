using System.Text;
using LogFileReaderLibrary.Models;
using LogFileReaderLibrary.Services;

namespace LogFileReaderConsoleApp.Handlers;

/// <summary>
/// Provides methods to analyze log files and report insights.
/// </summary>
public static class LogFileAnalyserHandler
{
    /// <summary>
    /// Reports insights from a list of <see cref="LogEntry"/> objects.
    /// </summary>
    /// <param name="logEntries"></param>
    /// <param name="top"></param>
    /// <returns>A string containing the insights.</returns>
    public static async Task<string> ReportInsights(IReadOnlyList<LogEntry> logEntries, int top = 3)
    {
        var uniqueIpCountTask = Task.Run(() => LogFileAnalyserService.UniqueIpCount(logEntries));
        var mostVisitedUrlsTask = Task.Run(() => LogFileAnalyserService.MostVisitedUrls(logEntries, top));
        var mostActiveIpsTask = Task.Run(() => LogFileAnalyserService.MostActiveIps(logEntries, top));

        await Task.WhenAll(uniqueIpCountTask, mostVisitedUrlsTask, mostActiveIpsTask);
        
        var output = new StringBuilder();
        output.AppendLine("Log File Insights:");
        output.AppendLine($"Unique IP Count: {uniqueIpCountTask.Result}");
        
        output.AppendLine("\nMost Visited URLs:");
        foreach (var url in mostVisitedUrlsTask.Result)
        {
            output.AppendLine($"URL: {url.Key}, Visits: {url.Value}");
        }
        
        output.AppendLine("\nMost Active IPs:");
        foreach (var ip in mostActiveIpsTask.Result)
        {
            output.AppendLine($"IP: {ip.Key}, Requests: {ip.Value}");
        }

        return output.ToString();
    }
}