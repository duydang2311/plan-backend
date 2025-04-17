using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Host.Migrations
{
    /// <inheritdoc />
    public partial class _079_notifications_workspacememberinvitedtrigger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                create function delete_workspace_invitation_notifications()
                returns trigger as
                $$
                begin
                    delete from "notifications" where ("data"->>'workspaceInvitationId')::bigint = old.id;
                    return old;
                END;
                $$
                language plpgsql;

                create TRIGGER TR_workspace_invitations_AD
                after delete on workspace_invitations
                for each row
                execute function delete_workspace_invitation_notifications();
                """
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                drop trigger TR_workspace_invitations_AD on issues;
                drop function delete_workspace_invitation_notifications();
                """
            );
        }
    }
}
