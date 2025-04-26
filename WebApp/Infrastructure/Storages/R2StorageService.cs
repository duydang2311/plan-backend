using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using WebApp.Infrastructure.Storages.Abstractions;

namespace WebApp.Infrastructure.Storages;

public sealed class R2StorageService(IAmazonS3 s3, IOptions<R2Options> r2Options) : IStorageService
{
    public Task<string> GetPreSignedUploadUrlAsync(string key, TimeSpan? expiration = null, string? contentType = null)
    {
        return s3.GetPreSignedURLAsync(
            new GetPreSignedUrlRequest
            {
                BucketName = r2Options.Value.BucketName,
                Key = key,
                Expires = DateTime.UtcNow + (expiration ?? TimeSpan.FromMinutes(5)),
                Verb = HttpVerb.PUT,
                ContentType = contentType,
            }
        );
    }
}
