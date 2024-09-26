using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace Hana.Core.Contracts;


public interface ISafeSerializer
{
    [RequiresUnreferencedCode("The type T may be trimmed.")]
    ValueTask<string> SerializeAsync(object? value, CancellationToken cancellationToken = default);


    [RequiresUnreferencedCode("The type T may be trimmed.")]
    ValueTask<JsonElement> SerializeToElementAsync(object? value, CancellationToken cancellationToken = default);


    [RequiresUnreferencedCode("The type T may be trimmed.")]
    ValueTask<T> DeserializeAsync<T>(string json, CancellationToken cancellationToken = default);


    [RequiresUnreferencedCode("The type T may be trimmed.")]
    ValueTask<T> DeserializeAsync<T>(JsonElement element, CancellationToken cancellationToken = default);
}