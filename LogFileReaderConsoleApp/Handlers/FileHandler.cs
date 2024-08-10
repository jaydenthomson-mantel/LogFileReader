using LogFileReaderLibrary.Helpers;
using LogFileReaderLibrary.Models;
using LogFileReaderLibrary.Models.Exceptions;

namespace LogFileReaderConsoleApp.Handlers;

public static class FileHandler
{
    public static bool TryReadFile(string filePath, out List<HttpRequestLogEntry> logEntries)
    {
        logEntries = [];

        try
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File not found: {filePath}");
                return false;
            }

            var content = File.OpenRead(filePath);
            logEntries = HttpRequestLogEntryDeserializer.DeserializeApacheClfList(content);
            return true;
        }
        catch (BadApacheClfFileException ex)
        {
            Console.WriteLine(ex.BadApacheClfFileExceptionErrorMessage);
            
            foreach (var innerException in ex.InnerExceptions)
            {
                if (innerException is ApacheClfLogValidationException aggregateException)
                {
                    Console.WriteLine($"\t{aggregateException.InvalidApacheClfLogLineErrorMessage}");
    
                    foreach (var nestedInnerException in aggregateException.InnerExceptions)
                    {
                        Console.WriteLine($"\t\t{nestedInnerException.Message}");
                    }
                }
                else
                {
                    Console.WriteLine($"\t{innerException.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unknown error reading file: {ex.Message}");
        }

        return false;
    }
}