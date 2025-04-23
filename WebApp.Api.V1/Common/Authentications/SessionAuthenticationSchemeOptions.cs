using Microsoft.AspNetCore.Authentication;

namespace WebApp.Api.V1.Common.Authentications;

public class SessionAuthenticationSchemeOptions : AuthenticationSchemeOptions
{
    public const string AuthenticationScheme = "Session";
    public const string AuthorizationHeaderName = "Authorization";
}
