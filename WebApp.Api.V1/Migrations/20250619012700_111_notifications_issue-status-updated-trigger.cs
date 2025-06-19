using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Host.Migrations
{
    /// <inheritdoc />
    public partial class _111_notifications_issuestatusupdatedtrigger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                create function delete_issue_status_updated_notifications()
                returns trigger as
                $$
                begin
                    delete from "notifications" where ("data"->>'oldStatusId')::bigint = old.id;
                    delete from "notifications" where ("data"->>'newStatusId')::bigint = old.id;
                    return old;
                END;
                $$
                language plpgsql;

                create TRIGGER TR_workspace_statuses_AD
                after delete on workspace_statuses
                for each row
                execute function delete_issue_status_updated_notifications();
                """
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                drop trigger TR_workspace_statuses_AD on workspace_statuses;
                drop function delete_issue_status_updated_notifications();
                """
            );
        }
    }
}
