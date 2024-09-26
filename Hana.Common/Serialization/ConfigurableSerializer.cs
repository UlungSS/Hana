using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Text.Unicode;
using Microsoft.Extensions.DependencyInjection;

using Hana.Common.Contracts;
using Hana.Common.Converters;

namespace Hana.Common.Serialization;

public abstract class ConfigurableSerializer(IServiceProvider serviceProvider)
{
    private JsonSerializerOptions? _options;

    protected IServiceProvider ServiceProvider { get; } = serviceProvider;

    public virtual JsonSerializerOptions GetOptions()
    {
        if (_options != null)
            return _options;

        var options = CreateOptionsInternal();
        ApplyOptions(options);
        _options = options;
        return options;
    }

    public virtual void ApplyOptions(JsonSerializerOptions options)
    {
        Configure(options);
        AddConverters(options);
        RunConfigurators(options);
    }

    protected JsonSerializerOptions GetOptionsInternal()
    {
        var options = CreateOptionsInternal();
        ApplyOptions(options);
        _options = options;
        return options;
    }

    private static JsonSerializerOptions CreateOptionsInternal()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        };

        options.Converters.Add(new JsonStringEnumConverter());
        options.Converters.Add(JsonMetadataServices.TimeSpanConverter);
        options.Converters.Add(new IntegerJsonConverter());
        options.Converters.Add(new DecimalJsonConverter());

        return options;
    }

    protected virtual void Configure(JsonSerializerOptions options)
    {
    }
    protected virtual void AddConverters(JsonSerializerOptions options)
    {
    }

    protected virtual void RunConfigurators(JsonSerializerOptions options)
    {
        var configurators = ServiceProvider.GetServices<ISerializationOptionsConfigurator>();
        var modifiers = new List<Action<JsonTypeInfo>>();

        foreach (var configurator in configurators)
        {
            configurator.Configure(options);
            var modifiersToAdd = configurator.GetModifiers();
            modifiers.AddRange(modifiersToAdd);
        }

        options.TypeInfoResolver = new ModifiableJsonTypeInfoResolver(modifiers);
    }

    protected T CreateInstance<T>() => ActivatorUtilities.CreateInstance<T>(ServiceProvider);
}