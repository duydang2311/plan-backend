using FastEndpoints;
using WebApp.Api.V1.Common;
using WebApp.Common.Constants;
using WebApp.Infrastructure.Caching.Common;

namespace WebApp.Api.V1.ProjectMembers.Create;

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
                    Permit.CreateProjectMember,
                    ct
                )
                .ConfigureAwait(false);
    }
}
