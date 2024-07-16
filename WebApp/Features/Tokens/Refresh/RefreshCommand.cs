using FastEndpoints;
using OneOf;
using WebApp.Domain.Entities;
using WebApp.SharedKernel.Models;

namespace WebApp.Features.Tokens.Refresh;

public sealed record class RefreshCommand(RefreshToken Token)
    : ICommand<OneOf<IEnumerable<ValidationError>, RefreshResult>> { }
