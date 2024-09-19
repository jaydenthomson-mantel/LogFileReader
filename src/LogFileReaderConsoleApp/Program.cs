using CommonLibrary;
using LogFileReaderConsoleApp.Handlers;
using LogFileReaderConsoleApp.Helpers;

namespace LogFileReaderConsoleApp;

public static class Program
{
    public static async Task Main(string[] args)
    {
        if (ConsoleArgumentsHelper.TryGetFilePath(args, out var filePath)
            && FileHandler.TryReadFile(filePath!, out var logEntries))
        {
            await LogFileAnalyserHandler.ReportInsights(logEntries);
        }

        var userId = args[1];
        SqlInjectionExample.GetUserData(userId);
    }
}