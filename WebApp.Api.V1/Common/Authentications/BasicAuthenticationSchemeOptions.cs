using Microsoft.AspNetCore.Authentication;

namespace WebApp.Api.V1.Common.Authentications;

public class BasicAuthenticationSchemeOptions : AuthenticationSchemeOptions
{
    public const string AuthenticationScheme = "Basic";
    public const string AuthorizationHeaderName = "Authorization";
}
