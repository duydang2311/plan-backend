namespace WebApp.Features.Users.SignProfileImageUpload;

public sealed record SignProfileImageUploadResult
{
    public required long Timestamp { get; init; }
    public required string Transformation { get; init; }
    public required string PublicId { get; init; }
    public required string NotificationUrl { get; init; }
    public required string Url { get; init; }
    public required string ApiKey { get; init; }
    public required string Signature { get; init; }
}
