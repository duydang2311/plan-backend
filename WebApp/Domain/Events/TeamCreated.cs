using WebApp.SharedKernel.Models;

namespace WebApp.Domain.Events;

public sealed record TeamCreated
{
    public required Team Team { get; init; }
    public required UserId UserId { get; init; }
}
