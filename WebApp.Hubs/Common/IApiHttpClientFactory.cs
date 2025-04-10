namespace WebApp.Hubs.Common;

public interface IApiHttpClientFactory
{
    HttpClient CreateClient();
}
