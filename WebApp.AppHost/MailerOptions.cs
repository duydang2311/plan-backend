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
}
