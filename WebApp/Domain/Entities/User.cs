using NodaTime;

namespace WebApp.Domain.Entities;

public sealed record class User
{
    public Instant CreatedTime { get; init; }
    public Instant UpdatedTime { get; init; }
    public UserId Id { get; init; } = UserId.Empty;
    public string Email { get; init; } = string.Empty;
    public byte[]? Salt { get; init; }
    public byte[]? PasswordHash { get; init; }
    public bool IsVerified { get; init; }
    public string Trigrams { get; init; } = null!;

    // Relationships
    public ICollection<Team> Teams { get; init; } = null!;
    public UserGoogleAuth? GoogleAuth { get; init; } = null!;
    public UserProfile? Profile { get; init; } = null!;
    public ICollection<Issue> Issues { get; init; } = null!;
    public ICollection<Workspace> Workspaces { get; init; } = null!;
    public ICollection<WorkspaceMember> WorkspaceMembers { get; init; } = null!;
    public ICollection<ProjectMember> ProjectMembers { get; init; } = null!;
    public ICollection<UserFriend> UserFriends { get; init; } = null!;
    public ICollection<UserFriendRequest> UserSentFriendRequests { get; init; } = null!;
    public ICollection<UserFriendRequest> UserReceivedFriendRequests { get; init; } = null!;
    public ICollection<ChatMember> ChatMembers { get; init; } = null!;
    public ICollection<Chat> Chats { get; init; } = null!;
    public ICollection<Chat> OwnedChats { get; init; } = null!;
}
