using System.Text.Json.Serialization;
using Json.Patch;

namespace WebApp.Api.V1.Commons;

[JsonSerializable(typeof(JsonPatch), GenerationMode = JsonSourceGenerationMode.Serialization)]
public sealed partial class ApiJsonSerializerContext : JsonSerializerContext { }
