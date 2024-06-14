using WebApp.SharedKernel.Models;

namespace WebApp.Features.Users.CreateUser;

public sealed record class CreateUserResult(User User, UserVerificationToken UserVerificationToken) { }
