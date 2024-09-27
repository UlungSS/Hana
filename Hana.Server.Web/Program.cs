using Hana.Core.Api.Middleware;
using Hana.Extensions;
using Hana.Users.Extensions;
using Microsoft.AspNetCore.ResponseCompression;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseStaticWebAssets();


// Add services to the container.
var services = builder.Services;
var configuration = builder.Configuration;

//Identity Section
var hanaIdentity = configuration.GetSection("Hana:Identity");
var hanaPersistence = configuration.GetSection("Hana:ProviderPersistence");
var hanaIdentityToken = hanaIdentity.GetSection("Tokens");


services.AddEndpointsApiExplorer();
services.AddControllers().AddJsonOptions(x =>
{
    // serialize enums as strings in api responses (e.g. Role)
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

    // ignore omitted parameters on models to enable optional params (e.g. User update)
    x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

services.AddHealthChecks();
services.AddCors(cors => cors.AddDefaultPolicy(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().WithExposedHeaders("*")));


#region RESPONSE COMPRESSION
services.AddResponseCompression(option =>
{
    //Can add a variety of compression types, the program will automatically get the best way according to the level
    option.EnableForHttps = true;

    //Use the compression policy for the specified mimeType
    option.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
    [
        // General
        "text/plain",

        // Static files
        "text/css",
        "application/javascript",
        "image/svg+xml",
        "application/atom+xml",
        "application/octet-stream",
        "application/font-woff",

        // MVC
        "text/html",
        "application/xml",
        "text/xml",
        "application/json",
        "text/json",
        "application/x-www-form-urlencoded"

    ]);

    //Add Compression Brotli (Faster)
    option.Providers.Add<BrotliCompressionProvider>();
});
#endregion


#region Hana
services.AddHana(hana =>
{
    hana.UseDefaultAuthentication()
   .UseIdentity(identity =>
   {
       identity.TokenOptions = options => hanaIdentityToken.Bind(options);
       identity.UseConfigurationBasedUserProvider(options => hanaIdentity.Bind(options));
       identity.UseConfigurationBasedRoleProvider(options => hanaIdentity.Bind(options));
       identity.UseConfigurationBasedApplicationProvider(options => hanaIdentity.Bind(options));
   });

    hana.UseHanaCoreApi(api =>
    {
        api.AddFastEndpointsAssembly<Program>();
    });

    //setting persistence provider
    hana.UseHanaCore(core => core.PersistenceOptions = options => hanaPersistence.Bind(options));
    
    //module
    hana.UseModuleUser();
    hana.AddSwagger();
});
#endregion


var app = builder.Build();

//https direction
app.UseHttpsRedirection();

//health check
app.MapHealthChecks("health");

//routing
app.UseRouting();

//cors
app.UseCors();
app.UseStaticFiles();

//security
app.UseAuthentication();
app.UseAuthorization();

//api hana
app.UseHanaApi();

// captures unhandled exceptions and returns a JSON response.
app.UseJsonSerializationErrorHandler();

// error handling middleware application
app.UseMiddleware<ErrorHandlerMiddleware>();

//swagger
app.UseSwaggerUI();

//response compression controllers
app.UseResponseCompression();

//controllers
app.MapControllers();

await app.RunAsync();
