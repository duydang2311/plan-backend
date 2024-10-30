using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Host.Migrations
{
    /// <inheritdoc />
    public partial class _048_issues_statusrank : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "order_by_status",
                table: "issues");

            migrationBuilder.AddColumn<string>(
                name: "status_rank",
                table: "issues",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status_rank",
                table: "issues");

            migrationBuilder.AddColumn<long>(
                name: "order_by_status",
                table: "issues",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
