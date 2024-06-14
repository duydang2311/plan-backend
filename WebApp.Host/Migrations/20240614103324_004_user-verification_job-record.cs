using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Host.Migrations
{
    /// <inheritdoc />
    public partial class _004_userverification_jobrecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "job_records",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    queue_id = table.Column<string>(type: "text", nullable: false),
                    execute_after = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    expire_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_complete = table.Column<bool>(type: "boolean", nullable: false),
                    command_json = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_job_records", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_verification_tokens",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    token = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_verification_tokens", x => new { x.user_id, x.token });
                    table.ForeignKey(
                        name: "fk_user_verification_tokens_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_user_verification_tokens_user_id",
                table: "user_verification_tokens",
                column: "user_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "job_records");

            migrationBuilder.DropTable(
                name: "user_verification_tokens");
        }
    }
}
