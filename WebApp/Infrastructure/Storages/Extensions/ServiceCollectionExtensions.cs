using CloudinaryDotNet;
using WebApp.Infrastructure.Storages.Abstractions;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddStorage(
        this IServiceCollection serviceCollection,
        CloudinaryOptions cloudinaryOptions
    )
    {
        serviceCollection.AddSingleton<ICloudinary>(_ => new Cloudinary(
            new Account
            {
                ApiKey = cloudinaryOptions.ApiKey,
                ApiSecret = cloudinaryOptions.ApiSecret,
                Cloud = cloudinaryOptions.CloudName,
            }
        ));
        return serviceCollection;
    }
}
