using System.Net;

namespace CommonLibrary.Helpers;

public static class HttpHelper
{
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