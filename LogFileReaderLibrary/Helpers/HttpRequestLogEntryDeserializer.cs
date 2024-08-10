using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;
using CommonLibrary.Helpers;
using LogFileReaderLibrary.Models;
using LogFileReaderLibrary.Models.Exceptions;

namespace LogFileReaderLibrary.Helpers;

/// <summary>
/// Helper class for deserializing http request logs.
/// </summary>
public static class HttpRequestLogEntryDeserializer
{
    private const string ApacheClfPattern =
        """^(?<ip>[\d\.]+) (?<identd>[\S]+) (?<userid>[\S]+) \[(?<timestamp>[^\]]+)\] "(?<method>[A-Z]+) (?<resource>[^ ]+) (?<httpversion>[^"]+)" (?<status>\d{3}) (?<size>\d+|-) "(?<referer>[^"]*)" "(?<useragent>[^"]*)"(?: (?<extra>.*))?$""";

    private static readonly Regex ApacheClfRegex =
        new(ApacheClfPattern, RegexOptions.CultureInvariant, TimeSpan.FromSeconds(5));

    /// <summary>
    /// Deserializes a stream of Apache Common Log Format (CLF) entries into a list of <see cref="HttpRequestLogEntry"/> objects.
    /// Sets <see cref="logContent"/> position to 0 before processing.
    /// </summary>
    /// <param name="logContent">A stream containing log entries in Apache CLF format.</param>
    /// <returns>A list of deserialized <see cref="HttpRequestLogEntry"/> objects.</returns>
    /// <exception cref="BadApacheClfFileException">Thrown when one or more log entries throw an exception.</exception>
    public static List<HttpRequestLogEntry> DeserializeApacheClfList(Stream logContent)
    {
        logContent.Position = 0;
        var logEntries = new List<HttpRequestLogEntry>();
        var exceptions = new List<Exception>();
        using var reader = new StreamReader(logContent);

        while (reader.ReadLine() is { } line)
        {
            try
            {
                var logEntry = DeserializeApacheClf(line);
                logEntries.Add(logEntry);
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }
        }

        if (exceptions.Count == 0)
        {
            return logEntries;
        }
        
        throw new BadApacheClfFileException(exceptions);
    }

    /// <summary>
    /// Deserializes a log entry in the Apache Common Log Format (CLF) into an <see cref="HttpRequestLogEntry"/> object.
    /// It also ignores any additional data at the end of the string.
    /// </summary>
    /// <param name="logEntry">A string representing a single log entry in the Apache CLF format.</param>
    /// <returns>An <see cref="HttpRequestLogEntry"/> object containing the parsed data from the log entry.</returns>
    /// <exception cref="ApacheClfLogValidationException">Thrown when one or more properties are invalid. The inner exceptions are of type <see cref="ValidationException"/>.</exception>
    /// <exception cref="FormatException">Thrown when log structure is in an unexpected format.</exception>
    /// <exception cref="InvalidOperationException">Thrown when a resource is found to be null after validation has been done.</exception>
    public static HttpRequestLogEntry DeserializeApacheClf(string logEntry)
    {
        var match = ApacheClfRegex.Match(logEntry);

        if (!match.Success)
        {
            throw new FormatException($"The log line '{logEntry}' is not in the expected format.");
        }

        var ipAddressStr = match.Groups["ip"].Value;
        var identd = match.Groups["identd"].Value;
        var userId = match.Groups["userid"].Value;
        var timestampStr = match.Groups["timestamp"].Value;
        var httpMethodStr = match.Groups["method"].Value;
        var resourceStr = match.Groups["resource"].Value;
        var httpVersion = match.Groups["httpversion"].Value;
        var statusCodeStr = match.Groups["status"].Value;
        var responseSizeStr = match.Groups["size"].Value;
        var referer = match.Groups["referer"].Value;
        var userAgent = match.Groups["useragent"].Value;

        var formatExceptions = new List<ValidationException>();

        if (!IPAddress.TryParse(ipAddressStr, out _))
        {
            formatExceptions.Add(new ValidationException($"'{ipAddressStr}' is an invalid IP address format."));
        }

        if (!HttpHelper.TryParseHttpStatusCode(statusCodeStr, out var statusCode))
        {
            formatExceptions.Add(new ValidationException($"'{statusCodeStr}' is an invalid status code format."));
        }

        if (!int.TryParse(responseSizeStr, out var responseSize))
        {
            formatExceptions.Add(new ValidationException($"'{responseSizeStr}' is an invalid response size format."));
        }

        if (!DateTimeOffset.TryParseExact(timestampStr, "dd/MMM/yyyy:HH:mm:ss zzz", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out var timestamp))
        {
            formatExceptions.Add(new ValidationException($"'{timestampStr}' is an invalid timestamp format."));
        }

        if (!Uri.TryCreate(resourceStr, UriKind.RelativeOrAbsolute, out var resource))
        {
            formatExceptions.Add(new ValidationException($"'{resourceStr}' is an invalid resource URI format."));
        }

        if (formatExceptions.Count > 0)
        {
            throw new ApacheClfLogValidationException(logEntry, formatExceptions);
        }

        return new HttpRequestLogEntry
        {
            IpAddress = ipAddressStr,
            Identd = identd,
            UserId = userId,
            Timestamp = timestamp,
            HttpMethod = HttpMethod.Parse(httpMethodStr),
            Resource = resource ?? throw new InvalidOperationException("Resource should not be null."),
            HttpVersion = httpVersion,
            StatusCode = statusCode,
            ResponseSize = responseSize,
            Referer = referer,
            UserAgent = userAgent
        };
    }
}