using LogFileReaderConsoleApp.Handlers;

if (args.Length < 1)
{
    Console.WriteLine("Provide a file path.");
    return;
}

var filePath = args[0];

if (FileHandler.TryReadFile(filePath, out var logEntries))
{
    LogFileAnalyserHandler.ReportInsights(logEntries);
}