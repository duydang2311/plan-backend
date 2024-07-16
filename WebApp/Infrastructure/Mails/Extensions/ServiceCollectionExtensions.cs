using WebApp.Infrastructure.Mails;
using WebApp.Infrastructure.Mails.Abstractions;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddMails(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IMailer, Mailer>();
        return serviceCollection;
    }
}
