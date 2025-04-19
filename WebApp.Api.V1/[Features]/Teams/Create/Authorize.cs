using FastEndpoints;
using WebApp.Api.V1.Common;
using WebApp.Common.Constants;
using WebApp.Infrastructure.Caching.Common;

namespace WebApp.Api.V1.Teams.Create;

public sealed class Authorize(IPermissionCache permissionCache) : AuthorizePreProcessor<Request>
{
    public override async Task<bool> AuthorizeAsync(
        Request request,
        IPreProcessorContext<Request> context,
        CancellationToken ct
    )
    {
        return request.WorkspaceId.HasValue
            && await permissionCache
                .HasWorkspacePermissionAsync(request.WorkspaceId.Value, request.UserId, Permit.CreateTeam, ct)
                .ConfigureAwait(false);
    }
}
