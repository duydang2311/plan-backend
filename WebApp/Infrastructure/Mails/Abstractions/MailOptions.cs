using System.ComponentModel.DataAnnotations;

namespace WebApp.Infrastructure.Mails.Abstractions;

public sealed record class MailOptions
{
    public const string Section = "Mail";

    [Required]
    public required string SmtpHost { get; init; }

    [Required]
    public required int SmtpPort { get; init; }

    [Required]
    public required string SmtpAuthUser { get; init; }

    [Required]
    public required string SmtpAuthPassword { get; init; }

    [Required]
    public required string SmtpName { get; init; }

    [Required]
    public required string SmtpAddress { get; init; }
}
