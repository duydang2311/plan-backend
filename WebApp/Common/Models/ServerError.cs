using WebApp.Common.Interfaces;

namespace WebApp.Common.Models;

public record ServerError(string Code) : IError { }
