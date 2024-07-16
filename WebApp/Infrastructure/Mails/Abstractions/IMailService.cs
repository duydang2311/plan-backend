using MailKit.Net.Smtp;

namespace WebApp.Infrastructure.Mails.Abstractions;

public interface IMailer
{
    Task<SmtpClient> CreateSmtpClientAsync(CancellationToken ct = default);
}
