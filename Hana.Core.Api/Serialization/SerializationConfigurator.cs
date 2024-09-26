using System.Text.Json;
using Hana.Core.Api.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Hana.Core.Api.Serialization;

internal class SerializationConfigurator(IServiceProvider serviceProvider) : SerializationOptionsConfiguratorBase
{
    public override void Configure(JsonSerializerOptions options)
    {
        options.Converters.Add(ActivatorUtilities.GetServiceOrCreateInstance<ArgumentJsonConverterFactory>(serviceProvider));
    }
}