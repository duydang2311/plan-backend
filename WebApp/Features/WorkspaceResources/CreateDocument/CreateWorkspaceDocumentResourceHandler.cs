using EntityFramework.Exceptions.Common;
using FastEndpoints;
using OneOf;
using OneOf.Types;
using WebApp.Common.Models;
using WebApp.Domain.Entities;
using WebApp.Features.WorkspaceResources.Common;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.WorkspaceResources.CreateDocument;

public sealed record CreateWorkspaceDocumentResourceHandler(AppDbContext db)
    : ICommandHandler<
        CreateWorkspaceDocumentResource,
        OneOf<WorkspaceNotFoundError, UserNotFoundError, InvalidResourceTypeError, Success>
    >
{
    public async Task<OneOf<WorkspaceNotFoundError, UserNotFoundError, InvalidResourceTypeError, Success>> ExecuteAsync(
        CreateWorkspaceDocumentResource command,
        CancellationToken ct
    )
    {
        var resource = new WorkspaceResource
        {
            WorkspaceId = command.WorkspaceId,
            Resource = new DocumentResource
            {
                CreatorId = command.CreatorId,
                Content = command.Content,
                Name = command.Name,
            },
        };

        db.Add(resource);
        try
        {
            await db.SaveChangesAsync(ct).ConfigureAwait(false);
            return new Success();
        }
        catch (ReferenceConstraintException e)
        {
            if (e.ConstraintProperties.Any(a => a == nameof(WorkspaceResource.WorkspaceId)))
            {
                return new WorkspaceNotFoundError();
            }
            if (e.ConstraintProperties.Any(a => a == nameof(DocumentResource.CreatorId)))
            {
                return new UserNotFoundError();
            }
            throw;
        }
    }
}
