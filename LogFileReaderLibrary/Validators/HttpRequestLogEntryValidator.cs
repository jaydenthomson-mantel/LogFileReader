using System.ComponentModel.DataAnnotations;
using LogFileReaderLibrary.Helpers;

namespace LogFileReaderLibrary.Validators;

/// <summary>
/// Validator for HTTP request log entries.
/// </summary>
public static class HttpRequestLogEntryValidator
{
    /// <summary>
    /// Validates the log content by reading each line and attempting to deserialize it. 
    /// If any line is in an unexpected format, a <see cref="ValidationException"/> is added to a list of exceptions.
    /// If no exceptions are encountered, the method returns true.
    /// If there are any exceptions, an <see cref="AggregateException"/> is thrown containing all the validation exceptions.
    /// </summary>
    /// <param name="logContent">A <see cref="Stream"/> containing the log content to be validated.</param>
    /// <returns>Returns <c>true</c> if all lines are successfully validated.</returns>
    /// <exception cref="AggregateException">Thrown when one or more lines in the log content are in an unexpected format.</exception>
    public static bool ValidateOrThrow(Stream logContent)
    {
        var exceptions = new List<Exception>();
        using var reader = new StreamReader(logContent);
        
        while (reader.ReadLine() is { } line)
        {
            try
            {
                HttpRequestLogEntryDeserializer.DeserializeApacheClf(line);
            }
            catch (Exception ex)
            {
                var validationException = new ValidationException($"Log '{line}' was in unexpected format.", ex);
                exceptions.Add(validationException);
            }
        }

        if (exceptions.Count == 0)
        {
            return true;
        }
        
        throw new AggregateException(exceptions);
    }
}