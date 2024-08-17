using System.Globalization;
using System.Text.Json;
using FastEndpoints;
using NATS.Client.Core;

namespace WebApp.Features.Users.SendVerificationMail;

public sealed class SendUserVerificationMailHandler(IServiceScopeFactory serviceScopeFactory)
    : ICommandHandler<SendUserVerificationMail>
{
    private const string Subject = "[plan] Email verification";
    private const string Body =
        "Thank you for signing up for plan. Please proceed to verify your email at the following URL: {0}";

    public async Task ExecuteAsync(SendUserVerificationMail command, CancellationToken ct)
    {
        using var scope = serviceScopeFactory.CreateAsyncScope();
        var nats = scope.ServiceProvider.GetRequiredService<INatsConnection>();
        await nats.PublishAsync(
                "mails.send",
                JsonSerializer.Serialize(
                    new
                    {
                        From = "Plan <plan.noreply@resend.dev>",
                        To = new string[] { command.Email },
                        Subject,
                        Text = string.Format(
                            CultureInfo.InvariantCulture,
                            Body,
                            command.VerificationUrl.Replace("{token}", command.Token.ToBase64())
                        ),
                    }
                ),
                cancellationToken: ct
            )
            .ConfigureAwait(false);
    }
}
