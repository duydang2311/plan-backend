using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Host.Migrations
{
    /// <inheritdoc />
    public partial class _013_issues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "issues",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<Guid>(
                name: "author_id",
                table: "issues",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "ix_issues_author_id",
                table: "issues",
                column: "author_id");

            migrationBuilder.AddForeignKey(
                name: "fk_issues_users_author_id",
                table: "issues",
                column: "author_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_issues_users_author_id",
                table: "issues");

            migrationBuilder.DropIndex(
                name: "ix_issues_author_id",
                table: "issues");

            migrationBuilder.DropColumn(
                name: "author_id",
                table: "issues");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "issues",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
