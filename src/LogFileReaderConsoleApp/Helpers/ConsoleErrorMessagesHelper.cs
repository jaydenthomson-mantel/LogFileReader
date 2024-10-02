using System.Text;
using LogFileReaderLibrary.Models.Exceptions;

namespace LogFileReaderConsoleApp.Helpers;

public static class ConsoleErrorMessagesHelper
{
    public static string GetBadApacheClfFileExceptionMessage(BadApacheClfFileException ex)
    {
        var errorMessage = new StringBuilder();
        errorMessage.AppendLine(BadApacheClfFileException.BadApacheClfFileExceptionErrorMessage);

        foreach (var innerException in ex.InnerExceptions)
        {
            if (innerException is ApacheClfLogValidationException aggregateException)
            {
                errorMessage.AppendLine($"\t{aggregateException.InvalidApacheClfLogLineErrorMessage}");

                foreach (var nestedInnerException in aggregateException.InnerExceptions)
                {
                    errorMessage.AppendLine($"\t\t{nestedInnerException.Message}");
                }
            }
            else
            {
                errorMessage.AppendLine($"\t{innerException.Message}");
            }
        }

        return errorMessage.ToString();
    }

}
