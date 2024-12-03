using CloudinaryDotNet;
using FastEndpoints;
using Microsoft.Extensions.Options;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Storages.Abstractions;

namespace WebApp.Features.Users.SignProfileImageUpload;

public sealed record SignProfileImageUploadHandler(
    ICloudinary cloudinary,
    IOptions<CloudinaryOptions> cloudinaryOptions
) : ICommandHandler<SignProfileImageUploadCommand, SignProfileImageUploadResult>
{
    public Task<SignProfileImageUploadResult> ExecuteAsync(SignProfileImageUploadCommand command, CancellationToken ct)
    {
        var cld = (Cloudinary)cloudinary;
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() / 1000;
        var transformation = "c_fill,h_256,w_256";
        var publicId = $"{command.UserId.ToBase64String()}/profile-image";
        var notificationUrl =
            $"{cloudinaryOptions.Value.WebhookOrigin}/api/users/{command.UserId.ToBase64String()}/profile/image-uploaded";
        var parameters = new SortedDictionary<string, object>()
        {
            ["timestamp"] = timestamp,
            ["transformation"] = transformation,
            ["public_id"] = publicId,
            ["notification_url"] = notificationUrl,
        };
        var signature = cld.Api.SignParameters(parameters);
        return Task.FromResult(
            new SignProfileImageUploadResult
            {
                Timestamp = timestamp,
                Transformation = transformation,
                PublicId = publicId,
                NotificationUrl = notificationUrl,
                Url = cld.Api.GetUploadUrl("image"),
                ApiKey = cloudinaryOptions.Value.ApiKey,
                Signature = signature,
            }
        );
    }
}
