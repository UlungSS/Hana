using Hana.Api.Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Hana.Core.Api.Middleware;

public class ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger _logger = logger;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            switch (error)
            {
                case AppException e:
                    // custom application error
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case KeyNotFoundException e:
                    // not found error
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                default:
                    // unhandled error
                    _logger.LogError(error, error.Message);
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            var errorResult = new DefaultResponse((int)RcCollection.Failed, error.Message, response.StatusCode, null!);

            var result = JsonSerializer.Serialize(errorResult);
            await response.WriteAsync(result);
        }
    }
}
