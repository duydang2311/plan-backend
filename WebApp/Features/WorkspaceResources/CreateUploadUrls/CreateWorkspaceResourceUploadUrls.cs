using FastEndpoints;
using OneOf;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.WorkspaceResources.CreateUploadUrls;

public sealed record CreateWorkspaceResourceUploadUrls
    : ICommand<OneOf<NotFoundError, ServerError, CreateWorkspaceResourceUploadUrlsResult>>
{
    public required WorkspaceId WorkspaceId { get; init; }
    public required string[] Keys { get; init; }
}
