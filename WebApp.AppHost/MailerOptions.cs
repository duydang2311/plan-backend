using System.ComponentModel.DataAnnotations;

namespace WebApp.AppHost;

public sealed record MailerOptions
{
    public const string Section = "Mailer";

    [Required]
    public required string Name { get; init; }

    [Required]
    public required string Image { get; init; }

    [Required]
    public required string Tag { get; init; }

    [Required]
    public required string DockerContextPath { get; init; }

    [Required]
    public required string ResendApiKey { get; init; }

    [Required]
    public required string NatsUrl { get; init; }

    [Required]
    public required string NatsUser { get; init; }

    [Required]
    public required string NatsPassword { get; init; }
}
