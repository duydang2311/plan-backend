using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Host.Migrations
{
    /// <inheritdoc />
    public partial class _107_milestones_endtimeidx_previewdescriptionrm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "fk_issues_milestones_milestone_id", table: "issues");

            migrationBuilder.DropColumn(name: "preview_description", table: "milestones");

            migrationBuilder.AlterColumn<string>(
                name: "color",
                table: "milestone_statuses",
                type: "character varying(64)",
                maxLength: 64,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64
            );

            migrationBuilder.CreateIndex(name: "ix_milestones_end_time", table: "milestones", column: "end_time");

            migrationBuilder.AddForeignKey(
                name: "fk_issues_milestones_milestone_id",
                table: "issues",
                column: "milestone_id",
                principalTable: "milestones",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "fk_issues_milestones_milestone_id", table: "issues");

            migrationBuilder.DropIndex(name: "ix_milestones_end_time", table: "milestones");

            migrationBuilder.AddColumn<string>(
                name: "preview_description",
                table: "milestones",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true
            );

            migrationBuilder.AlterColumn<string>(
                name: "color",
                table: "milestone_statuses",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64,
                oldNullable: true
            );

            migrationBuilder.AddForeignKey(
                name: "fk_issues_milestones_milestone_id",
                table: "issues",
                column: "milestone_id",
                principalTable: "milestones",
                principalColumn: "id"
            );
        }
    }
}
