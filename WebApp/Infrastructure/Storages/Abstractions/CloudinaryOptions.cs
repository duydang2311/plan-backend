using System.ComponentModel.DataAnnotations;

namespace WebApp.Infrastructure.Storages.Abstractions;

public sealed record CloudinaryOptions
{
    public const string Section = "Cloudinary";

    [Required]
    public required string ApiKey { get; init; }

    [Required]
    public required string ApiSecret { get; init; }

    [Required]
    public required string CloudName { get; init; }

    [Required]
    public required string WebhookOrigin { get; init; }
}
