using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Domain.Events;

public sealed record ProjectCreatedInline
{
    public required AppDbContext Db { get; init; }
    public required ProjectId ProjectId { get; init; }
}
