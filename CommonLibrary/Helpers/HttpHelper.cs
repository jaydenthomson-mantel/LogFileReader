using System.Net;

namespace CommonLibrary.Helpers;

/// <summary>
/// A helper class for HTTP-related operations.
/// </summary>
public static class HttpHelper
{
    /// <summary>
    /// Attempts to parse a string representation of an HTTP status code into an <see cref="HttpStatusCode"/> enum value.
    /// </summary>
    /// <param name="statusCodeStr">The string representation of the HTTP status code.</param>
    /// <param name="statusCode">
    /// When this method returns, contains the <see cref="HttpStatusCode"/> value equivalent to the HTTP status code 
    /// contained in <paramref name="statusCodeStr"/>, if the conversion succeeded, or the default value if the conversion failed.
    /// </param>
    /// <returns>
    /// <c>true</c> if <paramref name="statusCodeStr"/> was converted successfully; otherwise, <c>false</c>.
    /// </returns>
    public static bool TryParseHttpStatusCode(string statusCodeStr, out HttpStatusCode statusCode)
    {
        if (Enum.TryParse(statusCodeStr, out statusCode) && Enum.IsDefined(typeof(HttpStatusCode), statusCode))
        {
            return true;
        }

        statusCode = default;
        return false;
    }
}