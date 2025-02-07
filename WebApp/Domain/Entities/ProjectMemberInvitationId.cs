using System.Globalization;

namespace WebApp.Domain.Entities;

public readonly record struct ProjectMemberInvitationId : IEntityId<long>
{
    public long Value { get; init; }

    public static readonly ProjectMemberInvitationId Empty;

    public override string ToString()
    {
        return Value.ToString(CultureInfo.InvariantCulture);
    }
}
