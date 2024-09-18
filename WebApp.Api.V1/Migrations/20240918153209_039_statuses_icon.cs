using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Host.Migrations
{
    /// <inheritdoc />
    public partial class _039_statuses_icon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "icon",
                table: "workspace_statuses",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "icon",
                table: "workspace_statuses");
        }
    }
}
