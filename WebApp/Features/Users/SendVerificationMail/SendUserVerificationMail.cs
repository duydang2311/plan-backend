using FastEndpoints;

namespace WebApp.Features.Users.SendVerificationMail;

public sealed record class SendUserVerificationMail(string Email, string VerificationUrl, Guid Token) : ICommand { }
