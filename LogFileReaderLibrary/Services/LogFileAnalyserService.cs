namespace LogFileReaderLibrary.Services;

/// <summary>
/// This service analyses the contents of a log file.
/// </summary>
public class LogFileAnalyserService
{
    public int UniqueIpCount(Stream logContent)
    {
        throw new NotImplementedException();
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