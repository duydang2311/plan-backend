using Microsoft.Extensions.Options;

namespace WebApp.Hubs.Common;

public sealed class ApiHttpClientFactory(IHttpClientFactory httpClientFactory, IOptions<ApiOptions> apiOptions)
    : IApiHttpClientFactory
{
    public HttpClient CreateClient()
    {
        return httpClientFactory.CreateClient(apiOptions.Value.HttpClientName);
    }
}
