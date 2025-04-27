using System.Security.Claims;

namespace WebApp.Infrastructure.Jwts.Common;

public interface IJwtService
{
    public string GenerateJwt(string audience, IEnumerable<Claim> claims, DateTime expires);
}
