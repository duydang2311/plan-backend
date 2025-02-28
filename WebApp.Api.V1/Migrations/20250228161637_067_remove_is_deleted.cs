using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

#nullable disable

namespace WebApp.Host.Migrations
{
    /// <inheritdoc />
    public partial class _067_remove_is_deleted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "is_deleted", table: "issues");

            migrationBuilder.AddColumn<Instant>(
                name: "deleted_time",
                table: "issue_audits",
                type: "timestamp with time zone",
                nullable: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "deleted_time", table: "issue_audits");

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "issues",
                type: "boolean",
                nullable: false,
                defaultValue: false
            );
        }
    }
}
