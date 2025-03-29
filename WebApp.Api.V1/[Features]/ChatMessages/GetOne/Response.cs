using Riok.Mapperly.Abstractions;
using WebApp.Api.V1.Common.Dtos;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.ChatMessages.GetOne;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[UseStaticMapper(typeof(DtoMapper))]
public static partial class ResponseMapper
{
    public static partial BaseChatMessageDto ToResponse(this ChatMessage chatMessage);
}
