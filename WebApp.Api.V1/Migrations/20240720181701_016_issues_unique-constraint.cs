using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Host.Migrations
{
    /// <inheritdoc />
    public partial class _016_issues_uniqueconstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_issues_team_id",
                table: "issues");

            migrationBuilder.AlterColumn<long>(
                name: "order_number",
                table: "issues",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldDefaultValueSql: "0");

            migrationBuilder.CreateIndex(
                name: "ix_issues_team_id_order_number",
                table: "issues",
                columns: new[] { "team_id", "order_number" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_issues_team_id_order_number",
                table: "issues");

            migrationBuilder.AlterColumn<long>(
                name: "order_number",
                table: "issues",
                type: "bigint",
                nullable: false,
                defaultValueSql: "0",
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateIndex(
                name: "ix_issues_team_id",
                table: "issues",
                column: "team_id");
        }
    }
}
