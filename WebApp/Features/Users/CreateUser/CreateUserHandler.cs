using System.Security.Cryptography;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OneOf;
using WebApp.Domain.Entities;
using WebApp.SharedKernel.Hashers.Abstractions;
using WebApp.SharedKernel.Helpers;
using WebApp.SharedKernel.Models;
using WebApp.SharedKernel.Persistence;

namespace WebApp.Features.Users.CreateUser;

using Result = OneOf<IEnumerable<ValidationError>, CreateUserResult>;

public sealed class CreateUserHandler(IHasher hasher, AppDbContext dbContext)
    : ICommandHandler<CreateUserCommand, Result>
{
    public async Task<Result> ExecuteAsync(CreateUserCommand command, CancellationToken ct)
    {
        if (
            await dbContext
                .Users.AnyAsync(x => EF.Functions.Collate(x.Email, "case_insensitive") == command.Email, ct)
                .ConfigureAwait(false)
        )
        {
            return new[] { new ValidationError("email", "Email has already been used", "duplicated_email") };
        }

        var salt = RandomNumberGenerator.GetBytes(16);
        var user = new User
        {
            Email = command.Email,
            PasswordHash = hasher.Hash(command.Password, salt),
            Salt = salt,
        };
        var userVerificationToken = new UserVerificationToken { User = user, Token = IdHelper.NewGuid(), };

        dbContext.Add(user);
        dbContext.Add(userVerificationToken);
        await dbContext.SaveChangesAsync(ct).ConfigureAwait(false);
        return new CreateUserResult(user, userVerificationToken);
    }
}
