using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Host.Migrations
{
    /// <inheritdoc />
    public partial class _112_workspacemembers_unique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_workspace_members_workspace_id_user_id",
                table: "workspace_members",
                columns: new[] { "workspace_id", "user_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_workspace_members_workspace_id_user_id",
                table: "workspace_members");
        }
    }
}
