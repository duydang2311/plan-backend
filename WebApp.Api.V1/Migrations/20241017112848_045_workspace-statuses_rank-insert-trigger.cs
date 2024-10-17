using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Host.Migrations
{
    /// <inheritdoc />
    public partial class _045_workspacestatuses_rankinserttrigger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                    create function init_workspace_status() returns trigger as $$
                    begin
                        select coalesce(max(rank), 1 << 31) / 2 + ~(1 << 31) / 2 into new.rank from workspace_statuses where workspace_id = new.workspace_id;
                        return new;
                    end;
                    $$ language plpgsql;

                    create trigger TR_workspace_statuses_BI before insert on workspace_statuses
                    for each row execute function init_workspace_status();
                """
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                    drop trigger TR_workspace_statuses_BI on workspace_statuses;
                    drop function init_workspace_status();
                """
            );
        }
    }
}
