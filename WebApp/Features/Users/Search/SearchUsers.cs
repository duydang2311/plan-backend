using FastEndpoints;
using WebApp.Common.Models;

namespace WebApp.Features.Users.Search;

public sealed record SearchUsers : Collective, ICommand<SearchUsersResult>
{
    public required string Query { get; init; }
}
