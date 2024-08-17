using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Host.Migrations
{
    /// <inheritdoc />
    public partial class _024_teaminvitations_member : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_team_invitations_team_members_team_id_user_id",
                table: "team_invitations");

            migrationBuilder.DropForeignKey(
                name: "fk_team_invitations_users_user_id",
                table: "team_invitations");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "team_invitations",
                newName: "member_id");

            migrationBuilder.RenameIndex(
                name: "ix_team_invitations_user_id",
                table: "team_invitations",
                newName: "ix_team_invitations_member_id");

            migrationBuilder.AddForeignKey(
                name: "fk_team_invitations_team_members_team_id_member_id",
                table: "team_invitations",
                columns: new[] { "team_id", "member_id" },
                principalTable: "team_members",
                principalColumns: new[] { "team_id", "member_id" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_team_invitations_users_member_id",
                table: "team_invitations",
                column: "member_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_team_invitations_team_members_team_id_member_id",
                table: "team_invitations");

            migrationBuilder.DropForeignKey(
                name: "fk_team_invitations_users_member_id",
                table: "team_invitations");

            migrationBuilder.RenameColumn(
                name: "member_id",
                table: "team_invitations",
                newName: "user_id");

            migrationBuilder.RenameIndex(
                name: "ix_team_invitations_member_id",
                table: "team_invitations",
                newName: "ix_team_invitations_user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_team_invitations_team_members_team_id_user_id",
                table: "team_invitations",
                columns: new[] { "team_id", "user_id" },
                principalTable: "team_members",
                principalColumns: new[] { "team_id", "member_id" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_team_invitations_users_user_id",
                table: "team_invitations",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
