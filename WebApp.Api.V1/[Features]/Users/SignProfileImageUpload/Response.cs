using Riok.Mapperly.Abstractions;
using WebApp.Features.Users.SignProfileImageUpload;

namespace WebApp.Api.V1.Users.SignProfileImageUpload;

public sealed record Response
{
    public required long Timestamp { get; init; }
    public required string Transformation { get; init; }
    public required string PublicId { get; init; }
    public required string NotificationUrl { get; init; }
    public required string Url { get; init; }
    public required string ApiKey { get; init; }
    public required string Signature { get; init; }
}

[Mapper]
public static partial class ResponseMapper
{
    public static partial Response ToResponse(this SignProfileImageUploadResult result);
}
