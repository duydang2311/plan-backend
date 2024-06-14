using FastEndpoints;

namespace WebApp.Features.Emails.SendUserVerificationMail;

public sealed record class SendUserVerificationMailCommand(string Email, string VerificationUrl, Guid Token)
    : ICommand { }
