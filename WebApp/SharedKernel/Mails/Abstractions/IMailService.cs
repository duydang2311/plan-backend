using MailKit.Net.Smtp;

namespace WebApp.SharedKernel.Mails.Abstractions;

public interface IMailer
{
    Task<SmtpClient> CreateSmtpClientAsync(CancellationToken ct = default);
}
