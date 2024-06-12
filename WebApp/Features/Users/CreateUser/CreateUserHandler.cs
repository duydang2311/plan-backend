using System.Security.Cryptography;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OneOf;
using WebApp.SharedKernel.Hashers.Abstractions;
using WebApp.SharedKernel.Models;
using WebApp.SharedKernel.Persistence;

namespace WebApp.Features.Users.CreateUser;

using Result = OneOf<IEnumerable<ValidationError>, User>;

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

        dbContext.Add(user);
        await dbContext.SaveChangesAsync(ct).ConfigureAwait(false);
        return user;
    }
}
