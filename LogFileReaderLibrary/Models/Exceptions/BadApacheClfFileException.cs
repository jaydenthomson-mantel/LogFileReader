namespace LogFileReaderLibrary.Models.Exceptions;

public class BadApacheClfFileException(IEnumerable<Exception> innerExceptions)
    : AggregateException(innerExceptions)
{
    public string BadApacheClfFileExceptionErrorMessage => "One or more exceptions occured attempting to deserialize an Apache clf file. See the inner exceptions for more details.";
}