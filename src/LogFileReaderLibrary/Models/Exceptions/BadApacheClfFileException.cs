namespace LogFileReaderLibrary.Models.Exceptions;

/// <summary>
/// Represents an exception that occurs when attempting to deserialize an Apache clf file.
/// </summary>
/// <seealso cref="System.AggregateException" />
public class BadApacheClfFileException(IEnumerable<Exception> innerExceptions)
    : AggregateException(innerExceptions)
{
    /// <summary>
    /// Gets the error message that explains the reason for the exception.
    /// </summary>
    public static string BadApacheClfFileExceptionErrorMessage => "One or more exceptions occured attempting to deserialize an Apache clf file. See the inner exceptions for more details.";
}