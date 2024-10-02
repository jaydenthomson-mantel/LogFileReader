using System.Text;
using LogFileReaderConsoleApp.Helpers;
using LogFileReaderLibrary.Helpers;
using LogFileReaderLibrary.Models;
using LogFileReaderLibrary.Models.Exceptions;

namespace LogFileReaderConsoleApp.Handlers;

/// <summary>
/// Provides methods for handling file operations related to Apache Common Log Format (CLF) log entries.
/// </summary>
public static class FileHandler
{
    /// <summary>
    /// Attempts to read an Apache CLF log file and deserialize its content into a list of <see cref="LogEntry"/> objects.
    /// </summary>
    /// <param name="filePath">The path to the log file to be read.</param>
    /// <param name="logEntries">When this method returns, contains the list of deserialized <see cref="LogEntry"/> objects if the operation is successful; otherwise, an empty list.</param>
    /// <returns><c>true</c> if the file was successfully read and deserialized; otherwise, <c>false</c>.</returns>
    public static bool TryReadFile(string filePath, out IReadOnlyList<LogEntry> logEntries)
    {
        logEntries = Array.Empty<LogEntry>();

        if (!File.Exists(filePath))
        {
            Console.WriteLine($"File not found: {filePath}");
            return false;
        }

        try
        {
            using var content = File.OpenRead(filePath);
            logEntries = HttpRequestLogEntryDeserializer.DeserializeApacheClfList(content);
            return true;
        }
        catch (BadApacheClfFileException ex)
        {
            var errorMessage = ConsoleErrorMessagesHelper.GetBadApacheClfFileExceptionMessage(ex);
            Console.WriteLine(errorMessage);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unknown error reading file: {ex.Message}");
        }

        return false;
    }
}