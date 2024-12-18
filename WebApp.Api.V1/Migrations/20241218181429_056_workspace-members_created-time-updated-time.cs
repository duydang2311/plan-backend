using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

#nullable disable

namespace WebApp.Host.Migrations
{
    /// <inheritdoc />
    public partial class _056_workspacemembers_createdtimeupdatedtime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Instant>(
                name: "created_time",
                table: "workspace_members",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()"
            );

            migrationBuilder.AddColumn<Instant>(
                name: "updated_time",
                table: "workspace_members",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()"
            );

            migrationBuilder.Sql(
                """
                    create function update_workspace_member() returns trigger as $$
                    begin
                        new.updated_time := now();
                        return new;
                    end;
                    $$ language plpgsql;

                    create trigger TR_workspace_members_BU before update on workspace_members
                    for each row execute function update_workspace_member();
                """
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                    drop trigger TR_workspace_members_BU on workspace_members;
                    drop function update_workspace_member();
                """
            );
            migrationBuilder.DropColumn(name: "created_time", table: "workspace_members");

            migrationBuilder.DropColumn(name: "updated_time", table: "workspace_members");
        }
    }
}
