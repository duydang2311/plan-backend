using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using WebApp.Api.V1.Common.Dtos;

namespace WebApp.Api.V1.ChatMessages.GetOne;

using Results = Results<ForbidHttpResult, NotFound, Ok<BaseChatMessageDto>>;

public sealed class Endpoint : Endpoint<Request, Results>
{
    public override void Configure()
    {
        Get("chat-messages/{ChatMessageId}");
        Version(1);
        PreProcessor<Authorize>();
    }

    public override async Task<Results> ExecuteAsync(Request req, CancellationToken ct)
    {
        var oneOf = await req.ToCommand().ExecuteAsync(ct).ConfigureAwait(false);
        return oneOf.Match<Results>(
            notFoundError => TypedResults.NotFound(),
            chatMessage => TypedResults.Ok(chatMessage.ToResponse())
        );
    }
}
