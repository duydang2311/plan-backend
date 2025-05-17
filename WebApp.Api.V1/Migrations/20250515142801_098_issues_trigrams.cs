using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Host.Migrations
{
    /// <inheritdoc />
    public partial class _098_issues_trigrams : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "trigrams",
                table: "issues",
                type: "text",
                nullable: false,
                computedColumnSql: "\"order_number\"::text || ' ' || \"title\" || ' ' || \"description\"",
                stored: true);

            migrationBuilder.CreateIndex(
                name: "ix_issues_trigrams",
                table: "issues",
                column: "trigrams")
                .Annotation("Npgsql:IndexMethod", "gin")
                .Annotation("Npgsql:IndexOperators", new[] { "gin_trgm_ops" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_issues_trigrams",
                table: "issues");

            migrationBuilder.DropColumn(
                name: "trigrams",
                table: "issues");
        }
    }
}
