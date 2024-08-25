using WebApp.Domain.Constants;

namespace WebApp.Domain.Entities;

public record FieldDefinition
{
    public string Name { get; init; } = string.Empty;
    public FieldType Type { get; init; }
    public string? Description { get; init; }
}
