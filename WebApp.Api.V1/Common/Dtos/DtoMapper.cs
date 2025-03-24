using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.Common.Dtos;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class DtoMapper
{
    public static partial BaseUserDto? UserToDtoInternal(User? user);

    public static BaseUserDto? UserToDto(User user) => UserToDtoInternal(user);

    public static partial BaseUserProfileDto? UserProfileToDtoInternal(UserProfile? userProfile);

    public static BaseUserProfileDto? UserProfileToDto(UserProfile userProfile) =>
        UserProfileToDtoInternal(userProfile);

    public static partial BaseChatDto? ChatToDtoInternal(Chat? chat);

    public static BaseChatDto? ChatToDto(Chat chat) => ChatToDtoInternal(chat);

    public static partial BaseChatMessageDto? ChatMessageToDtoInternal(ChatMessage? chatMessage);

    public static BaseChatMessageDto? ChatMessageToDto(ChatMessage chatMessage) =>
        ChatMessageToDtoInternal(chatMessage);
}
