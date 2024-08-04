using System.ComponentModel.DataAnnotations;

namespace WebApp.Infrastructure.Nats.Abstractions;

public sealed record NatsOptions
{
    public const string Section = "NATS";

    [Required]
    public required string Url { get; init; }

    [Required]
    public required string Username { get; init; }

    [Required]
    public required string Password { get; init; }
}
