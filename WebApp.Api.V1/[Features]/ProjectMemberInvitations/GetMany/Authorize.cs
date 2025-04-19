using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebApp.Api.V1.Common;
using WebApp.Common.Constants;
using WebApp.Infrastructure.Caching.Common;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Api.V1.ProjectMemberInvitations.GetMany;

public sealed class Authorize(IPermissionCache permissionCache) : AuthorizePreProcessor<Request>
{
    public override async Task<bool> AuthorizeAsync(
        Request request,
        IPreProcessorContext<Request> context,
        CancellationToken ct
    )
    {
        return request.ProjectId.HasValue
            && await permissionCache
                .HasProjectPermissionAsync(
                    request.ProjectId.Value,
                    request.RequestingUserId,
                    Permit.ReadProjectMemberInvitation,
                    ct
                )
                .ConfigureAwait(false);
    }
}
