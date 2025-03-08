using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Host.Migrations
{
    /// <inheritdoc />
    public partial class _070_usernotifications_projectmemberinvited : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                create function delete_project_member_invited_notifications()
                returns trigger as
                $$
                begin
                    delete from "notifications" where ("data"->>'projectMemberInvitationId')::bigint = old.id;
                    return old;
                END;
                $$
                language plpgsql;

                create TRIGGER TR_project_member_invitations_AD
                after delete on project_member_invitations
                for each row
                execute function delete_project_member_invited_notifications();
                """
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                drop trigger TR_project_member_invitations_AD on project_member_invitations;
                drop function delete_project_member_invited_notifications();
                """
            );
        }
    }
}
