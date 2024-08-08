using System.Globalization;
using System.Text.RegularExpressions;
using LogFileReaderLibrary.Models;

namespace LogFileReaderLibrary.Helpers;

/// <summary>
/// Helper class for deserializing http request logs.
/// </summary>
public static class HttpRequestLogEntryDeserializer
{
    private const string ApacheClfPattern = @"^(?<ip>[\d\.]+) (?<identd>[\S]+) (?<userid>[\S]+) \[(?<timestamp>[^\]]+)\] ""(?<method>[A-Z]+) (?<resource>[^ ]+) (?<httpversion>[^""]+)"" (?<status>\d{3}) (?<size>\d+|-) ""(?<referer>[^""]*)"" ""(?<useragent>[^""]*)""(?: (?<extra>.*))?$";
    private static readonly Regex ApacheClfRegex = new (ApacheClfPattern, RegexOptions.CultureInvariant, TimeSpan.FromSeconds(5));
    
    /// <summary>
    /// Deserializes a log entry in the Apache Common Log Format (CLF) into an <see cref="HttpRequestLogEntry"/> object.
    /// It also ignores any additional data at the end of the string.
    /// </summary>
    /// <param name="logEntry">A string representing a single log entry in the Apache CLF format.</param>
    /// <returns>An <see cref="HttpRequestLogEntry"/> object containing the parsed data from the log entry.</returns>
    /// <exception cref="FormatException">Thrown when the log entry does not match the expected Apache CLF format.</exception>
    public static HttpRequestLogEntry DeserializeApacheClf(string logEntry)
    {
        var match = ApacheClfRegex.Match(logEntry);

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