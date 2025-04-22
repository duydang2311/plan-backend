using System.ComponentModel.DataAnnotations;

namespace WebApp.Infrastructure.Storages.Abstractions;

public sealed record R2Options
{
    public const string Section = "R2";

    [Required]
    public required string BucketName { get; init; }

    [Required]
    public required string S3AccessKeyId { get; init; }

    [Required]
    public required string S3SecretAccessKey { get; init; }

    [Required]
    public required string S3Endpoint { get; init; }
}
