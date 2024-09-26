using System.Text.Json;
using System.Text.Json.Serialization;

using Hana.Expressions.Contracts;

using Hana.Core.Models;

namespace Hana.Core.Api.Serialization;

internal class ArgumentJsonConverterFactory(IWellKnownTypeRegistry wellKnownTypeRegistry) : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert) => typeToConvert.IsAssignableTo(typeof(ArgumentDefinition));

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        return new ArgumentJsonConverter(wellKnownTypeRegistry);
    }
}