using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Host.Migrations
{
    /// <inheritdoc />
    public partial class _099_issues_trigramsweighted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "trigrams",
                table: "issues",
                type: "text",
                nullable: false,
                computedColumnSql: "repeat(\"order_number\"::text || ' ', 4) || \"title\" || ' '",
                stored: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldComputedColumnSql: "\"order_number\"::text || ' ' || \"title\" || ' ' || \"description\"",
                oldStored: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "trigrams",
                table: "issues",
                type: "text",
                nullable: false,
                computedColumnSql: "\"order_number\"::text || ' ' || \"title\" || ' ' || \"description\"",
                stored: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldComputedColumnSql: "repeat(\"order_number\"::text || ' ', 4) || \"title\" || ' '",
                oldStored: true);
        }
    }
}
