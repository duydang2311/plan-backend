namespace WebApp.SharedKernel.Models;

public sealed record Policy
{
    public long Id { get; init; }
    public string Subject { get; init; } = string.Empty;
    public string Object { get; init; } = string.Empty;
    public string Action { get; init; } = string.Empty;
    public string? Domain { get; init; }
}
