using Hana.Common.Serialization;
using Hana.Core.Contracts;
using Hana.Core.Converters;
using System.Text.Json;

namespace Hana.Core.Serializer;

public class ApiSerializer(IServiceProvider serviceProvider) : ConfigurableSerializer(serviceProvider), IApiSerializer
{

    public string Serialize(object model)
    {
        var options = GetOptions();
        return JsonSerializer.Serialize(model, options);
    }

    public object Deserialize(string serializedModel) => Deserialize<object>(serializedModel);

    public T Deserialize<T>(string serializedModel)
    {
        var options = GetOptions();
        return JsonSerializer.Deserialize<T>(serializedModel, options)!;
    }

    protected override void Configure(JsonSerializerOptions options)
    {
        options.PropertyNameCaseInsensitive = true;
    }

    protected override void AddConverters(JsonSerializerOptions options)
    {
        options.Converters.Add(CreateInstance<TypeJsonConverter>());
    }

    JsonSerializerOptions IApiSerializer.GetOptions() => GetOptions();

    JsonSerializerOptions IApiSerializer.ApplyOptions(JsonSerializerOptions options)
    {
        ApplyOptions(options);
        return options;
    }
}
