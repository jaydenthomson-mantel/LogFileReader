using System.Globalization;
using System.Text.RegularExpressions;
using LogFileReaderLibrary.Models;

namespace LogFileReaderLibrary.Helpers;

public static class HttpRequestLogEntryDeserializer
{
    public static HttpRequestLogEntry Deserialize(string logEntry)
    {
        const string pattern = @"^(?<ip>[\d\.]+) (?<identd>[\S]+) (?<userid>[\S]+) \[(?<timestamp>[^\]]+)\] ""(?<method>[A-Z]+) (?<resource>[^ ]+) (?<httpversion>[^""]+)"" (?<status>\d{3}) (?<size>\d+|-) ""(?<referer>[^""]*)"" ""(?<useragent>[^""]*)""$";
        var match = Regex.Match(logEntry, pattern); // TODO refactor regex class to be a static class (better performance)

        if (!match.Success)
        {
            throw new FormatException("The log line is not in the expected format.");
        }

        var ipAddress = match.Groups["ip"].Value;
        var identd = match.Groups["identd"].Value;
        var userId = match.Groups["userid"].Value;
        var timestampStr = match.Groups["timestamp"].Value;
        var httpMethod = match.Groups["method"].Value;
        var resource = match.Groups["resource"].Value;
        var httpVersion = match.Groups["httpversion"].Value;
        var statusCode = int.Parse(match.Groups["status"].Value);
        var responseSize = match.Groups["size"].Value == "-" ? 0 : int.Parse(match.Groups["size"].Value);
        var referer = match.Groups["referer"].Value;
        var userAgent = match.Groups["useragent"].Value;

        var timestamp = DateTimeOffset.ParseExact(timestampStr, "dd/MMM/yyyy:HH:mm:ss zzz", CultureInfo.InvariantCulture);

        return new HttpRequestLogEntry
        {
            IpAddress = ipAddress,
            Identd = identd,
            UserId = userId,
            Timestamp = timestamp,
            HttpMethod = httpMethod,
            Resource = resource,
            HttpVersion = httpVersion,
            StatusCode = statusCode,
            ResponseSize = responseSize,
            Referer = referer,
            UserAgent = userAgent
        };
    }
}