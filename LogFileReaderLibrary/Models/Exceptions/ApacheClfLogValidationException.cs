namespace LogFileReaderLibrary.Models.Exceptions;

public class ApacheClfLogValidationException(string logLine, IEnumerable<Exception> innerExceptions)
    : AggregateException(innerExceptions)
{
    public string InvalidApacheClfLogLineErrorMessage => $"One or more properties in the log line '{logLine}' were invalid.";
}