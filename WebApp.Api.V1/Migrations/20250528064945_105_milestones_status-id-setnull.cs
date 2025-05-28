using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Host.Migrations
{
    /// <inheritdoc />
    public partial class _105_milestones_statusidsetnull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "fk_milestones_milestone_statuses_status_id", table: "milestones");

            migrationBuilder.AddForeignKey(
                name: "fk_milestones_milestone_statuses_status_id",
                table: "milestones",
                column: "status_id",
                principalTable: "milestone_statuses",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "fk_milestones_milestone_statuses_status_id", table: "milestones");

            migrationBuilder.AddForeignKey(
                name: "fk_milestones_milestone_statuses_status_id",
                table: "milestones",
                column: "status_id",
                principalTable: "milestone_statuses",
                principalColumn: "id"
            );
        }
    }
}
