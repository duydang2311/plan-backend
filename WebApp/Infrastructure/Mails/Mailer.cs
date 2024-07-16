using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using WebApp.Infrastructure.Mails.Abstractions;

namespace WebApp.Infrastructure.Mails;

public sealed class Mailer(IOptions<MailOptions> options) : IMailer
{
    public async Task<SmtpClient> CreateSmtpClientAsync(CancellationToken ct = default)
    {
        var client = new SmtpClient();
        await client
            .ConnectAsync(options.Value.SmtpHost, options.Value.SmtpPort, false, cancellationToken: ct)
            .ConfigureAwait(false);
        await client
            .AuthenticateAsync(options.Value.SmtpAuthUser, options.Value.SmtpAuthPassword, cancellationToken: ct)
            .ConfigureAwait(false);
        return client;
    }
}
