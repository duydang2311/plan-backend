using WebApp.Common.Interfaces;

namespace WebApp.Common.Models;

public record ServerError(string Code) : IError
{
    public static ServerError From(string code) => new(code);
}
