using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace Hana.Core.Api.Middleware;


public class JsonSerializationErrorHandlerMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        var method = httpContext.Request.Method;

        // If the HTTP verb is anything but POST or PUT, do nothing.
        if (!string.Equals(method, "POST", StringComparison.InvariantCultureIgnoreCase) && !string.Equals(method, "PUT", StringComparison.InvariantCultureIgnoreCase))
        {
            await next(httpContext);
            return;
        }

        try
        {
            await next(httpContext);
        }
        catch (JsonException e)
        {
            var model = new
            {
                ModelBindingError = e.Message
            };

            var result = Results.BadRequest(model);

            await result.ExecuteAsync(httpContext);
        }
    }
}