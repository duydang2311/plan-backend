using WebApp.Common.Models;

namespace WebApp.Common.Helpers;

public static class Errors
{
    public static ServerError Outbox() => new("outbox");
}
