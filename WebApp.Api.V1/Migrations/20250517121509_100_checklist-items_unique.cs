using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Host.Migrations
{
    /// <inheritdoc />
    public partial class _100_checklistitems_unique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(name: "ix_checklist_items_parent_issue_id", table: "checklist_items");

            migrationBuilder.CreateIndex(
                name: "ix_checklist_items_parent_issue_id_sub_issue_id",
                table: "checklist_items",
                columns: new[] { "parent_issue_id", "sub_issue_id" },
                unique: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_checklist_items_parent_issue_id_sub_issue_id",
                table: "checklist_items"
            );

            migrationBuilder.CreateIndex(
                name: "ix_checklist_items_parent_issue_id",
                table: "checklist_items",
                column: "parent_issue_id"
            );
        }
    }
}
