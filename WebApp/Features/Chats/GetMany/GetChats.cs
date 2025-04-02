using FastEndpoints;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.Chats.GetMany;

public sealed record GetChats : Collective, ICommand<PaginatedList<Chat>>
{
    public UserId? UserId { get; init; }
    public string? Select { get; init; }
    public string? SelectLastChatMessage { get; init; }
    public UserId? FilterChatMemberId { get; init; }
    public string? SelectChatMember { get; init; }
}
