using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Host.Migrations
{
    /// <inheritdoc />
    public partial class _036_projectissues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_project_issues",
                table: "project_issues");

            migrationBuilder.DropIndex(
                name: "ix_project_issues_project_id",
                table: "project_issues");

            migrationBuilder.AddPrimaryKey(
                name: "pk_project_issues",
                table: "project_issues",
                columns: new[] { "project_id", "issue_id" });

            migrationBuilder.CreateIndex(
                name: "ix_project_issues_issue_id",
                table: "project_issues",
                column: "issue_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_project_issues",
                table: "project_issues");

            migrationBuilder.DropIndex(
                name: "ix_project_issues_issue_id",
                table: "project_issues");

            migrationBuilder.AddPrimaryKey(
                name: "pk_project_issues",
                table: "project_issues",
                columns: new[] { "issue_id", "project_id" });

            migrationBuilder.CreateIndex(
                name: "ix_project_issues_project_id",
                table: "project_issues",
                column: "project_id");
        }
    }
}
