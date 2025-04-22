using Amazon.S3;
using Microsoft.Extensions.Options;
using WebApp.Infrastructure.Storages.Abstractions;

namespace WebApp.Infrastructure.Storages;

public sealed class R2StorageService(IAmazonS3 s3, IOptions<R2Options> r2Options) : IStorageService
{
    public string GeneratePreSignedUploadUrl(
        string key,
        TimeSpan? expiration = null,
        IDictionary<string, object>? additionalProperties = null
    )
    {
        return s3.GeneratePreSignedURL(
            r2Options.Value.BucketName,
            key,
            expiration: DateTime.UtcNow + (expiration ?? TimeSpan.FromMinutes(5)),
            additionalProperties
        );
    }
}
