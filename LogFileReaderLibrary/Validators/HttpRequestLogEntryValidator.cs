using System.ComponentModel.DataAnnotations;
using LogFileReaderLibrary.Helpers;
using LogFileReaderLibrary.Services;
using Microsoft.Extensions.Logging;

namespace LogFileReaderLibrary.Validators;

/// <summary>
/// Validates the 
/// </summary>
public class HttpRequestLogEntryValidator(ILogger<HttpRequestLogEntryValidator> logger)
{
    public bool Validate(Stream logContent)
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
        
        var aggregateException = new AggregateException(exceptions);
        logger.LogError(aggregateException, "Log file was in incorrect format.");
        return false;
    }
}