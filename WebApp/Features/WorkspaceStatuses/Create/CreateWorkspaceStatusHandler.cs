using EntityFramework.Exceptions.Common;
using FastEndpoints;
using OneOf;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.WorkspaceStatuses.Create;

public sealed class CreateWorkspaceStatusHandler(AppDbContext db)
    : ICommandHandler<CreateWorkspaceStatus, OneOf<ValidationFailures, WorkspaceStatus>>
{
    public async Task<OneOf<ValidationFailures, WorkspaceStatus>> ExecuteAsync(
        CreateWorkspaceStatus command,
        CancellationToken ct
    )
    {
        var status = new WorkspaceStatus
        {
            WorkspaceId = command.WorkspaceId,
            Category = command.Category,
            Value = command.Value,
            Description = command.Description,
        };
        db.Add(status);

        try
        {
            await db.SaveChangesAsync(ct).ConfigureAwait(false);
        }
        catch (ReferenceConstraintException)
        {
            return ValidationFailures.Single("workspaceId", "Workspace does not exist", "no_reference");
        }

        return status;
    }
}
