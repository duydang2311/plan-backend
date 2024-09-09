using FastEndpoints;
using OneOf;
using WebApp.Common.Models;
using WebApp.Domain.Entities;

namespace WebApp.Features.Users.CreateWithGoogle;

public sealed record class CreateUserWithGoogleCommand(string IdToken) : ICommand<OneOf<ValidationFailures, User>> { }
