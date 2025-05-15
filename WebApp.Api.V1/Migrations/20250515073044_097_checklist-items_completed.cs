using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Host.Migrations
{
    /// <inheritdoc />
    public partial class _097_checklistitems_completed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "kind",
                table: "checklist_items",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<bool>(
                name: "completed",
                table: "checklist_items",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddCheckConstraint(
                name: "CHK_valid_sub_issue",
                table: "checklist_items",
                sql: "(\"kind\" = 2 AND \"sub_issue_id\" IS NOT NULL) OR (\"kind\" != 2 AND \"sub_issue_id\" IS NULL)");

            migrationBuilder.AddCheckConstraint(
                name: "CHK_valid_todo",
                table: "checklist_items",
                sql: "(\"kind\" = 1 AND \"content\" IS NOT NULL AND \"completed\" IS NOT NULL AND \"sub_issue_id\" IS NULL) OR (\"kind\" != 1 AND \"content\" IS NULL AND \"completed\" IS NULL)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CHK_valid_sub_issue",
                table: "checklist_items");

            migrationBuilder.DropCheckConstraint(
                name: "CHK_valid_todo",
                table: "checklist_items");

            migrationBuilder.DropColumn(
                name: "completed",
                table: "checklist_items");

            migrationBuilder.AlterColumn<int>(
                name: "kind",
                table: "checklist_items",
                type: "integer",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "smallint");
        }
    }
}
