using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Host.Migrations
{
    /// <inheritdoc />
    public partial class _025_teaminvitations_id : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_team_invitations",
                table: "team_invitations");

            migrationBuilder.AddColumn<Guid>(
                name: "id",
                table: "team_invitations",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "pk_team_invitations",
                table: "team_invitations",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "ix_team_invitations_team_id_member_id",
                table: "team_invitations",
                columns: new[] { "team_id", "member_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_team_invitations",
                table: "team_invitations");

            migrationBuilder.DropIndex(
                name: "ix_team_invitations_team_id_member_id",
                table: "team_invitations");

            migrationBuilder.DropColumn(
                name: "id",
                table: "team_invitations");

            migrationBuilder.AddPrimaryKey(
                name: "pk_team_invitations",
                table: "team_invitations",
                columns: new[] { "team_id", "member_id" });
        }
    }
}
