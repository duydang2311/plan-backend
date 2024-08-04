using Microsoft.AspNetCore.Authentication;

namespace WebApp.Domain.Entities;

public static class EntityGuidExtensions
{
    public static string ToBase64String(this IEntityGuid entityGuid)
    {
        return Base64UrlTextEncoder.Encode(entityGuid.Value.ToByteArray());
    }
}
