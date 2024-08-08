using System.ComponentModel.DataAnnotations;
using LogFileReaderLibrary.Helpers;

namespace LogFileReaderLibrary.Services;

/// <summary>
/// This service analyses the contents of a log file.
/// </summary>
public class LogFileAnalyserService
{
    public int UniqueIpCount(Stream logContent)
    {
        var hashSet = new HashSet<string>();
        using var reader = new StreamReader(logContent);
        
        while (reader.ReadLine() is { } line)
        {
            var logEntry = HttpRequestLogEntryDeserializer.DeserializeApacheClf(line);
            hashSet.Add(logEntry.IpAddress);
        }

        return hashSet.Count;
    }

    public IDictionary<string, int> MostVisitedUrls(Stream logContent, int top)
    {
        throw new NotImplementedException();
    }

    public IDictionary<string, int> MostActiveIps(Stream logContent, int top)
    {
        throw new NotImplementedException();
    }
}