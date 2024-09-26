using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Builder;

using Hana.Features.Services;

namespace Hana.Extensions;

public static class SwaggerExtensions
{
    public static IModule AddSwagger(this IModule module)
    {
        Version ver = new(1, 0);

        // Swagger API documentation
        module.Services.SwaggerDocument(o =>
        {
            o.EnableJWTBearerAuth = true;
            o.DocumentSettings = s =>
            {
                s.DocumentName = $"v{ver.Major}";
                s.Title = "Hana API";
                s.Version = $"v{ver.Major}.{ver.Minor}";
                s.AddAuth("ApiKey", new()
                {
                    Name = "Authorization",
                    In = NSwag.OpenApiSecurityApiKeyLocation.Header,
                    Type = NSwag.OpenApiSecuritySchemeType.ApiKey,
                    Description = "Enter: ApiKey [your API key]"
                });
            };
        });

        return module;
    }

    public static IApplicationBuilder UseSwaggerUI(this IApplicationBuilder app)
    {
        return app.UseSwaggerGen();
    }

}