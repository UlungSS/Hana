using System.Text.Json;

namespace Hana.Expressions.Models;

public record ExpressionSerializationContext(string ExpressionType, JsonElement JsonElement, JsonSerializerOptions Options, Type MemoryBlockType);