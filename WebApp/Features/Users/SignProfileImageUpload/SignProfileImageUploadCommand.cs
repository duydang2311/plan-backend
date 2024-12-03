using FastEndpoints;
using WebApp.Domain.Entities;

namespace WebApp.Features.Users.SignProfileImageUpload;

public sealed record SignProfileImageUploadCommand : ICommand<SignProfileImageUploadResult>
{
    public required UserId UserId { get; init; }
}
