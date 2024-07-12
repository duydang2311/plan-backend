using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

#nullable disable

namespace WebApp.Host.Migrations
{
    /// <inheritdoc />
    public partial class _010_teammember : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_team_members_teams_teams_id",
                table: "team_members");

            migrationBuilder.DropForeignKey(
                name: "fk_team_members_users_members_id",
                table: "team_members");

            migrationBuilder.DropIndex(
                name: "ix_teams_workspace_id",
                table: "teams");

            migrationBuilder.RenameColumn(
                name: "teams_id",
                table: "team_members",
                newName: "member_id");

            migrationBuilder.RenameColumn(
                name: "members_id",
                table: "team_members",
                newName: "team_id");

            migrationBuilder.RenameIndex(
                name: "ix_team_members_teams_id",
                table: "team_members",
                newName: "ix_team_members_member_id");

            migrationBuilder.AlterColumn<string>(
                name: "identifier",
                table: "teams",
                type: "character varying(5)",
                maxLength: 5,
                nullable: false,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<Instant>(
                name: "created_time",
                table: "team_members",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<Instant>(
                name: "updated_time",
                table: "team_members",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.CreateIndex(
                name: "ix_teams_workspace_id_identifier",
                table: "teams",
                columns: new[] { "workspace_id", "identifier" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_team_members_teams_team_id",
                table: "team_members",
                column: "team_id",
                principalTable: "teams",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_team_members_users_member_id",
                table: "team_members",
                column: "member_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_team_members_teams_team_id",
                table: "team_members");

            migrationBuilder.DropForeignKey(
                name: "fk_team_members_users_member_id",
                table: "team_members");

            migrationBuilder.DropIndex(
                name: "ix_teams_workspace_id_identifier",
                table: "teams");

            migrationBuilder.DropColumn(
                name: "created_time",
                table: "team_members");

            migrationBuilder.DropColumn(
                name: "updated_time",
                table: "team_members");

            migrationBuilder.RenameColumn(
                name: "member_id",
                table: "team_members",
                newName: "teams_id");

            migrationBuilder.RenameColumn(
                name: "team_id",
                table: "team_members",
                newName: "members_id");

            migrationBuilder.RenameIndex(
                name: "ix_team_members_member_id",
                table: "team_members",
                newName: "ix_team_members_teams_id");

            migrationBuilder.AlterColumn<string>(
                name: "identifier",
                table: "teams",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(5)",
                oldMaxLength: 5,
                oldCollation: "case_insensitive");

            migrationBuilder.CreateIndex(
                name: "ix_teams_workspace_id",
                table: "teams",
                column: "workspace_id");

            migrationBuilder.AddForeignKey(
                name: "fk_team_members_teams_teams_id",
                table: "team_members",
                column: "teams_id",
                principalTable: "teams",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_team_members_users_members_id",
                table: "team_members",
                column: "members_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
