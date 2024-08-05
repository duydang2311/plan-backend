using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using WebApp.Common.Helpers;
using WebApp.Domain.Entities;
using WebApp.Infrastructure.Persistence;

namespace WebApp.Features.IssueComments.GetOne;

public sealed class GetIssueCommandHandler(AppDbContext db)
    : ICommandHandler<GetIssueComment, OneOf<None, IssueComment>>
{
    public async Task<OneOf<None, IssueComment>> ExecuteAsync(GetIssueComment command, CancellationToken ct)
    {
        var query = db.IssueComments.Where(x => x.Id == command.IssueCommentId);
        if (!string.IsNullOrEmpty(command.Select))
        {
            query = query.Select(ExpressionHelper.LambdaNew<IssueComment>(command.Select));
        }

        var comment = await query.FirstOrDefaultAsync(ct).ConfigureAwait(false);
        return comment is null ? new None() : comment;
    }
}
