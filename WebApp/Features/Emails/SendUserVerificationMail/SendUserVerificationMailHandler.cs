using System.Globalization;
using FastEndpoints;
using Microsoft.Extensions.Options;
using MimeKit;
using WebApp.SharedKernel.Mails.Abstractions;

namespace WebApp.Features.Emails.SendUserVerificationMail;

public sealed class SendUserVerificationMailHandler(IMailer mailer, IOptions<MailOptions> options)
    : ICommandHandler<SendUserVerificationMailCommand>
{
    private const string Subject = "[plan] Email verification";
    private const string Body =
        "Thank you for signing up for plan. Please proceed to verify your email at the following URL: {0}";

    public async Task ExecuteAsync(SendUserVerificationMailCommand command, CancellationToken ct)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(options.Value.SmtpName, options.Value.SmtpAddress));
        message.To.Add(new MailboxAddress(command.Email, command.Email));
        message.Subject = Subject;
        message.Body = new TextPart("plain")
        {
            Text = string.Format(
                CultureInfo.InvariantCulture,
                Body,
                command.VerificationUrl.Replace("{token}", command.Token.ToBase64())
            )
        };

        using var client = await mailer.CreateSmtpClientAsync(ct).ConfigureAwait(false);
        await client.SendAsync(message, ct).ConfigureAwait(false);
        await client.DisconnectAsync(true, ct).ConfigureAwait(false);
    }
}
