using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebApp.Host.Migrations
{
    /// <inheritdoc />
    public partial class _064_projectmemberinvitations_Id : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_project_member_invitations",
                table: "project_member_invitations");

            migrationBuilder.DropIndex(
                name: "ix_project_member_invitations_user_id",
                table: "project_member_invitations");

            migrationBuilder.AlterColumn<Instant>(
                name: "created_time",
                table: "project_members",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()",
                oldClrType: typeof(Instant),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<Instant>(
                name: "created_time",
                table: "project_member_invitations",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()",
                oldClrType: typeof(Instant),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<long>(
                name: "id",
                table: "project_member_invitations",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "pk_project_member_invitations",
                table: "project_member_invitations",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "ix_project_member_invitations_user_id_project_id",
                table: "project_member_invitations",
                columns: new[] { "user_id", "project_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_project_member_invitations",
                table: "project_member_invitations");

            migrationBuilder.DropIndex(
                name: "ix_project_member_invitations_user_id_project_id",
                table: "project_member_invitations");

            migrationBuilder.DropColumn(
                name: "id",
                table: "project_member_invitations");

            migrationBuilder.AlterColumn<Instant>(
                name: "created_time",
                table: "project_members",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(Instant),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "now()");

            migrationBuilder.AlterColumn<Instant>(
                name: "created_time",
                table: "project_member_invitations",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(Instant),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "now()");

            migrationBuilder.AddPrimaryKey(
                name: "pk_project_member_invitations",
                table: "project_member_invitations",
                columns: new[] { "user_id", "project_id" });

            migrationBuilder.CreateIndex(
                name: "ix_project_member_invitations_user_id",
                table: "project_member_invitations",
                column: "user_id");
        }
    }
}
