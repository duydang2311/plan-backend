using WebApp.Domain.Constants;

namespace WebApp.Domain.Entities;

public abstract record Status
{
    public StatusId Id { get; init; }
    public StatusCategory Category { get; init; }
    public int Rank { get; init; }
    public string Value { get; init; } = string.Empty;
    public string Color { get; init; } = string.Empty;
    public string? Icon { get; init; }
    public string? Description { get; init; }
}
