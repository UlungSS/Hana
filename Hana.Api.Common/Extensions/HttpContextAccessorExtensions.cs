using System.Net;
using Microsoft.AspNetCore.Http;

namespace Hana.Extensions;

public static class HttpRequestExtensions
{
    public static bool IsLocal(this HttpRequest request)
    {
        var connection = request.HttpContext.Connection;
        return connection.RemoteIpAddress != null
            ? connection.LocalIpAddress != null
                ? connection.RemoteIpAddress.Equals(connection.LocalIpAddress)
                : IPAddress.IsLoopback(connection.RemoteIpAddress)
            : connection.RemoteIpAddress == null && connection.LocalIpAddress == null;
    }
}