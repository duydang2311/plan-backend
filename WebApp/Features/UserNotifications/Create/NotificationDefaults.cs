using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebApp.Features.UserNotifications.Create;

public static class NotificationDefaults
{
    public static readonly JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.General)
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };
}
