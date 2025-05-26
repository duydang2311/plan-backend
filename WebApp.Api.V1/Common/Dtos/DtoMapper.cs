using Riok.Mapperly.Abstractions;
using WebApp.Domain.Entities;

namespace WebApp.Api.V1.Common.Dtos;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class DtoMapper
{
    private static partial BaseUserDto? UserToDtoInternal(User? user);

    private static partial IReadOnlyCollection<BaseUserDto>? UsersToDtoInternal(IReadOnlyCollection<User>? users);

#pragma warning disable IDE0051 // Remove unused private members
    private static partial IReadOnlyCollection<BaseChatMemberDto>? ChatMembersToDtoInternal(
#pragma warning restore IDE0051 // Remove unused private members
        IReadOnlyCollection<ChatMember>? chatMembers
    );

    public static BaseUserDto? UserToDto(User user) => UserToDtoInternal(user);

    public static IReadOnlyCollection<BaseUserDto>? UsersToDto(IReadOnlyCollection<User> users) =>
        UsersToDtoInternal(users);

    public static partial BaseUserProfileDto? UserProfileToDto(UserProfile? userProfile);

    private static partial BaseChatDto? ChatToDtoInternal(Chat? chat);

    public static BaseChatDto? ChatToDto(Chat chat) => ChatToDtoInternal(chat);

    private static partial BaseChatMessageDto? ChatMessageToDtoInternal(ChatMessage? chatMessage);

    public static BaseChatMessageDto? ChatMessageToDto(ChatMessage chatMessage) =>
        ChatMessageToDtoInternal(chatMessage);

    private static partial BaseChatMemberDto? ChatMemberToDtoInternal(ChatMember? chatMember);

    public static BaseChatMemberDto? ChatMemberToDto(ChatMember chatMember) => ChatMemberToDtoInternal(chatMember);

    private static partial BaseWorkspaceDto? WorkspaceToDtoInternal(Workspace? Workspace);

    public static BaseWorkspaceDto? WorkspaceToDto(Workspace workspace) => WorkspaceToDtoInternal(workspace);

    private static partial BaseWorkspaceInvitationDto? WorkspaceInvitationToDtoInternal(
        WorkspaceInvitation? workspaceInvitation
    );

    public static BaseWorkspaceInvitationDto? WorkspaceInvitationToDto(WorkspaceInvitation workspaceInvitation) =>
        WorkspaceInvitationToDtoInternal(workspaceInvitation);

    private static partial BaseResourceDto? ResourceToDtoInternal(Resource? resource);

    public static BaseResourceDto? ResourceToDto(Resource resource) => ResourceToDtoInternal(resource);

    private static partial BaseWorkspaceResourceDto? WorkspaceResourceToDtoInternal(
        WorkspaceResource? workspaceResource
    );

    public static BaseWorkspaceResourceDto? WorkspaceResourceToDto(WorkspaceResource workspaceResource) =>
        WorkspaceResourceToDtoInternal(workspaceResource);

    public static partial IReadOnlyCollection<BaseWorkspaceResourceDto> WorkspaceResourcesToDto(
        IReadOnlyCollection<WorkspaceResource> workspaceResources
    );

    private static partial BaseResourceDocumentDto? ResourceDocumentToDtoInternal(ResourceDocument? resourceDocument);

    public static BaseResourceDocumentDto? ResourceDocumentToDto(ResourceDocument resourceDocument) =>
        ResourceDocumentToDtoInternal(resourceDocument);

    private static partial BaseResourceFileDto? ResourceFileToDtoInternal(ResourceFile? resourceFile);

    public static BaseResourceFileDto? ResourceFileToDto(ResourceFile resourceFile) =>
        ResourceFileToDtoInternal(resourceFile);

    private static partial ICollection<BaseResourceFileDto>? ResourceFilesToDtoInternal(
        ICollection<ResourceFile>? resourceFiles
    );

    public static ICollection<BaseResourceFileDto>? ResourceFilesToDto(ICollection<ResourceFile> resourceFiles) =>
        ResourceFilesToDtoInternal(resourceFiles);

    public static partial IReadOnlyCollection<BaseResourceFileDto> ResourceFilesToDto(
        IReadOnlyCollection<ResourceFile> resourceFiles
    );

    private static partial IReadOnlyCollection<string>? ResourceFileMimeTypesToDtoInternal(
        IReadOnlyCollection<string>? resourceFileMimeTypes
    );

    public static IReadOnlyCollection<string>? ResourceFileMimeTypesToDto(
        IReadOnlyCollection<string> resourceFileMimeTypes
    ) => ResourceFileMimeTypesToDtoInternal(resourceFileMimeTypes);

    private static partial BaseWorkspaceStatusDto? WorkspaceStatusToDtoInternal(WorkspaceStatus? workspaceStatus);

    public static BaseWorkspaceStatusDto? WorkspaceStatusToDto(WorkspaceStatus workspaceStatus) =>
        WorkspaceStatusToDtoInternal(workspaceStatus);

    private static partial BaseIssueDto? IssueToDtoInternal(Issue? issue);

    public static BaseIssueDto? IssueToDto(Issue issue) => IssueToDtoInternal(issue);

    private static partial BaseTeamDto? TeamToDtoInternal(Team? team);

    public static BaseTeamDto? TeamToDto(Team team) => TeamToDtoInternal(team);

    private static partial ICollection<BaseTeamDto>? TeamsToDtoInternal(ICollection<Team>? teams);

    public static ICollection<BaseTeamDto>? TeamsToDto(ICollection<Team> teams) => TeamsToDtoInternal(teams);

    private static partial ICollection<BaseUserDto>? UsersToDtoInternal(ICollection<User>? users);

    public static ICollection<BaseUserDto>? UsersToDto(ICollection<User> users) => UsersToDtoInternal(users);

    public static partial IReadOnlyCollection<BaseIssueDto> IssuesToDto(IReadOnlyCollection<Issue> issues);

    private static partial ICollection<BaseIssueAssigneeDto>? IssueAssigneesToDtoInternal(
        ICollection<IssueAssignee>? issueAssignees
    );

    public static ICollection<BaseIssueAssigneeDto>? IssueAssigneesToDto(ICollection<IssueAssignee> issueAssignees) =>
        IssueAssigneesToDtoInternal(issueAssignees);

    public static partial IReadOnlyCollection<BaseIssueAssigneeDto> IssueAssigneesToDto(
        IReadOnlyCollection<IssueAssignee> issueAssignees
    );

    private static partial ICollection<BaseTeamIssueDto>? TeamIssuesToDtoInternal(ICollection<TeamIssue>? teamIssues);

    public static ICollection<BaseTeamIssueDto>? TeamIssuesToDto(ICollection<TeamIssue> teamIssues) =>
        TeamIssuesToDtoInternal(teamIssues);

    public static partial IReadOnlyCollection<BaseTeamIssueDto> TeamIssuesToDto(
        IReadOnlyCollection<TeamIssue> teamIssues
    );

    private static partial BaseChecklistItemDto? ChecklistItemToDtoInternal(ChecklistItem? checklistItem);

    public static BaseChecklistItemDto? ChecklistItemToDto(ChecklistItem checklistItem) =>
        ChecklistItemToDtoInternal(checklistItem);

    private static partial ICollection<BaseUserSocialLinkDto>? UserSocialLinksToDtoInternal(
        ICollection<UserSocialLink>? userSocialLinks
    );

    public static ICollection<BaseUserSocialLinkDto>? UserSocialLinksToDto(
        ICollection<UserSocialLink> userSocialLinks
    ) => UserSocialLinksToDtoInternal(userSocialLinks);

    public static partial BaseMilestoneDto MilestoneToDto(Milestone milestone);
}
