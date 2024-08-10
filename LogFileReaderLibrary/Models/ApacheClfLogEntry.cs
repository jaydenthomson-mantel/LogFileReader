using System.Net;

namespace LogFileReaderLibrary.Models;

public class ApacheClfLogEntry
{
    public required string IpAddress { get; init; }
    public required string Identd { get; init; }
    public required string UserId { get; init; }
    public required DateTimeOffset Timestamp { get; init; }
    public required HttpMethod HttpMethod { get; init; }
    public required string HttpVersion { get; init; }
    public required Uri Resource { get; init; }
    public required HttpStatusCode StatusCode { get; init; }
    public required int ResponseSize { get; init; }
    public required string Referer { get; init; }
    public required string UserAgent { get; init; }
}