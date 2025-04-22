using Amazon.S3;
using CloudinaryDotNet;
using Microsoft.Extensions.Options;
using WebApp.Infrastructure.Storages;
using WebApp.Infrastructure.Storages.Abstractions;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddStorage(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<ICloudinary>(provider =>
        {
            var options = provider.GetRequiredService<IOptions<CloudinaryOptions>>().Value;
            return new Cloudinary(
                new Account
                {
                    ApiKey = options.ApiKey,
                    ApiSecret = options.ApiSecret,
                    Cloud = options.CloudName,
                }
            );
        });

        serviceCollection.AddSingleton<IAmazonS3>(
            (provider) =>
            {
                var options = provider.GetRequiredService<IOptions<R2Options>>().Value;
                return new AmazonS3Client(
                    options.S3AccessKeyId,
                    options.S3SecretAccessKey,
                    new AmazonS3Config { ServiceURL = options.S3Endpoint }
                );
            }
        );

        serviceCollection.AddSingleton<IStorageService, R2StorageService>();
        return serviceCollection;
    }
}
