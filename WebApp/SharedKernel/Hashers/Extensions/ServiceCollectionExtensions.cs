using WebApp.SharedKernel.Hashers;
using WebApp.SharedKernel.Hashers.Abstractions;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddHashers(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IHasher, ScryptHasher>();
        return serviceCollection;
    }
}
