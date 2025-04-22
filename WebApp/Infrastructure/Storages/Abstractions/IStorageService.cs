namespace WebApp.Infrastructure.Storages.Abstractions;

public interface IStorageService
{
    string GeneratePreSignedUploadUrl(
        string key,
        TimeSpan? expiration = null,
        IDictionary<string, object>? additionalProperties = null
    );
}
