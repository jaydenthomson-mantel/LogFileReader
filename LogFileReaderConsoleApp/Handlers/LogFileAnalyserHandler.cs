using LogFileReaderLibrary.Models;
using LogFileReaderLibrary.Services;

namespace LogFileReaderConsoleApp.Handlers;

public static class LogFileAnalyserHandler
{
    public static void ReportInsights(List<HttpRequestLogEntry> logEntries, int top = 3)
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