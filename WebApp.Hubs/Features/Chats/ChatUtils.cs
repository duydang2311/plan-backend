namespace WebApp.Hubs.Features.Chats;

public static class ChatUtils
{
    public static string GroupName(string chatId) => $"chat:{chatId}";
}
