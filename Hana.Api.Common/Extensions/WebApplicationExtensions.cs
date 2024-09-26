using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

using FastEndpoints;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

using Hana.Core.Contracts;

// ReSharper disable once CheckNamespace
namespace Hana.Extensions;

[PublicAPI]
public static class WebApplicationExtensions
{
    public static IApplicationBuilder UseHanaApi(this IApplicationBuilder app, string routePrefix = "hana/api")
    {
        return app.UseFastEndpoints(config =>
        {
            config.Endpoints.RoutePrefix = routePrefix;
            config.Serializer.RequestDeserializer = DeserializeRequestAsync;
            config.Serializer.ResponseSerializer = SerializeRequestAsync;

            config.Binding.ValueParserFor<DateTimeOffset>(s =>
                new(DateTimeOffset.TryParse(s!.ToString(), CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var result), result));
        });
    }

    public static IEndpointRouteBuilder MapHanaApi(this IEndpointRouteBuilder routes, string routePrefix = "hana/api") =>
        routes.MapFastEndpoints(config =>
        {
            config.Endpoints.RoutePrefix = routePrefix;
            config.Serializer.RequestDeserializer = DeserializeRequestAsync;
            config.Serializer.ResponseSerializer = SerializeRequestAsync;
        });

    private static ValueTask<object?> DeserializeRequestAsync(HttpRequest httpRequest, Type modelType, JsonSerializerContext? serializerContext, CancellationToken cancellationToken)
    {
        var serializer = httpRequest.HttpContext.RequestServices.GetRequiredService<IApiSerializer>();
        var options = serializer.GetOptions();

        return serializerContext == null
            ? JsonSerializer.DeserializeAsync(httpRequest.Body, modelType, options, cancellationToken)
            : JsonSerializer.DeserializeAsync(httpRequest.Body, modelType, serializerContext, cancellationToken);
    }

    private static Task SerializeRequestAsync(HttpResponse httpResponse, object? dto, string contentType, JsonSerializerContext? serializerContext, CancellationToken cancellationToken)
    {
        var serializer = httpResponse.HttpContext.RequestServices.GetRequiredService<IApiSerializer>();
        var options = serializer.GetOptions();

        httpResponse.ContentType = contentType;
        return serializerContext == null
            ? JsonSerializer.SerializeAsync(httpResponse.Body, dto, dto?.GetType() ?? typeof(object), options, cancellationToken)
            : JsonSerializer.SerializeAsync(httpResponse.Body, dto, dto?.GetType() ?? typeof(object), serializerContext, cancellationToken);
    }

}