namespace LogFileReaderConsoleApp.Helpers;

/// <summary>
/// A helper class for handling console arguments.
/// </summary>
public static class ConsoleArgumentsHelper
{
    /// <summary>
    /// Tries to get the file path from the console arguments.
    /// </summary>
    /// <param name="args">The console arguments.</param>
    /// <param name="filePath">
    /// When this method returns, contains the file path if the arguments contain a valid file path; otherwise, <c>null</c>.
    /// </param>
    /// <returns>
    /// <c>true</c> if the arguments contain a valid file path; otherwise, <c>false</c>.
    /// </returns>
    public static bool TryGetFilePath(string[] args, out string? filePath)
    {
        if (args.Length < 1)
        {
            Console.WriteLine("Provide a file path.");
            filePath = default;
            return false;
        }

        filePath = args[0];
        return true;
    }
}