using WebApp.Common.IdEncoding;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddCommon(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddOptions<IdEncoderOptions>()
            .BindConfiguration(IdEncoderOptions.Section)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        serviceCollection.AddSingleton<IIdEncoder, SqidsEncoder>();
        return serviceCollection;
    }
}
