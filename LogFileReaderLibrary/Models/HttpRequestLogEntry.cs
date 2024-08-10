namespace LogFileReaderLibrary.Models;

public class HttpRequestLogEntry
{
    public string IpAddress { get; set; }
    public string Identd { get; set; }
    public string UserId { get; set; }
    public DateTimeOffset Timestamp { get; set; }
    public HttpMethod HttpMethod { get; set; }
    public string HttpVersion { get; set; }
    public int StatusCode { get; set; }
    public int ResponseSize { get; set; }
    public string Referer { get; set; }
    public string UserAgent { get; set; }
}