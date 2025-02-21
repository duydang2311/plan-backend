namespace WebApp.Domain.Constants;

public enum NotificationStatus : uint
{
    None,
    Sent = 0b01,
    Read = 0b10,
}
