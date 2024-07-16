using FastEndpoints;
using OneOf;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.Tokens.Refresh;

public sealed record class RefreshCommand(RefreshToken Token)
    : ICommand<OneOf<IEnumerable<ValidationError>, RefreshResult>> { }
