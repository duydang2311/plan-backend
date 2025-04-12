using System.Text.Json.Serialization;

namespace WebApp.Infrastructure.Messaging;

[JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Serialization)]
[JsonSerializable(typeof(ChatMessageCreatedPayload))]
[JsonSerializable(typeof(IssueCommentCreatedPayload))]
[JsonSerializable(typeof(IssueCreatedPayload))]
[JsonSerializable(typeof(ProjectCreatedPayload))]
[JsonSerializable(typeof(ProjectMemberInvitedPayload))]
internal partial class MessagingJsonContext : JsonSerializerContext;
