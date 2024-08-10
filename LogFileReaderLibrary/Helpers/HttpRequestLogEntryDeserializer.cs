using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;
using LogFileReaderLibrary.Models;

namespace LogFileReaderLibrary.Helpers;

/// <summary>
/// Helper class for deserializing http request logs.
/// </summary>
public static class HttpRequestLogEntryDeserializer
{
    private const string ApacheClfPattern = """^(?<ip>[\d\.]+) (?<identd>[\S]+) (?<userid>[\S]+) \[(?<timestamp>[^\]]+)\] "(?<method>[A-Z]+) (?<resource>[^ ]+) (?<httpversion>[^"]+)" (?<status>\d{3}) (?<size>\d+|-) "(?<referer>[^"]*)" "(?<useragent>[^"]*)"(?: (?<extra>.*))?$""";
    private static readonly Regex ApacheClfRegex = new (ApacheClfPattern, RegexOptions.CultureInvariant, TimeSpan.FromSeconds(5));

    /// <summary>
    /// Deserializes a stream of Apache Common Log Format (CLF) entries into a list of <see cref="HttpRequestLogEntry"/> objects.
    /// </summary>
    /// <param name="logContent">A stream containing log entries in Apache CLF format.</param>
    /// <returns>A list of deserialized <see cref="HttpRequestLogEntry"/> objects.</returns>
    /// <exception cref="AggregateException">Thrown when one or more log entries are in an unexpected format.</exception>
    public static List<HttpRequestLogEntry> DeserializeApacheClfList(Stream logContent)
    {
        var logEntries = new List<HttpRequestLogEntry>();
        var exceptions = new List<Exception>();
        using var reader = new StreamReader(logContent);
        
        while (reader.ReadLine() is { } line)
        {
            try
            {
                var logEntry = HttpRequestLogEntryDeserializer.DeserializeApacheClf(line);
                logEntries.Add(logEntry);
            }
            catch (Exception ex)
            {
                var validationException = new ValidationException($"Log '{line}' was in unexpected format.", ex);
                exceptions.Add(validationException);
            }
        }

        if (exceptions.Count == 0)
        {
            return logEntries;
        }
        
        throw new AggregateException(exceptions);
    }
    
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
        var responseSize = int.Parse(match.Groups["size"].Value);
        var referer = match.Groups["referer"].Value;
        var userAgent = match.Groups["useragent"].Value;

        var timestamp = DateTimeOffset.ParseExact(timestampStr, "dd/MMM/yyyy:HH:mm:ss zzz", CultureInfo.InvariantCulture);

        return new HttpRequestLogEntry
        {
            IpAddress = ipAddress,
            Identd = identd,
            UserId = userId,
            Timestamp = timestamp,
            HttpMethod = HttpMethod.Parse(httpMethod),
            Resource = new Uri(resource),
            HttpVersion = httpVersion,
            StatusCode = statusCode,
            ResponseSize = responseSize,
            Referer = referer,
            UserAgent = userAgent
        };
    }
}