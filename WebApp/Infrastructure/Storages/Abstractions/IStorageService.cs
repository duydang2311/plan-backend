namespace WebApp.Infrastructure.Storages.Abstractions;

public interface IStorageService
{
    Task<string> GetPreSignedUploadUrlAsync(string key, TimeSpan? expiration = null, string? contentType = null);
    Task DeleteAsync(string key, CancellationToken ct = default);
}
