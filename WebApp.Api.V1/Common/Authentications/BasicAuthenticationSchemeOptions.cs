using Microsoft.AspNetCore.Authentication;

namespace WebApp.Api.V1.Common.Authentications;

public class BasicAuthenticationSchemeOptions : AuthenticationSchemeOptions
{
    public const string DefaultScheme = "Basic";
    public const string AuthorizationHeaderName = "Authorization";
}
