using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Host.Migrations
{
    /// <inheritdoc />
    public partial class _076_chats_owner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "owner_id",
                table: "chats",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000")
            );

            migrationBuilder.CreateIndex(name: "ix_chats_owner_id", table: "chats", column: "owner_id");

            migrationBuilder.AddForeignKey(
                name: "fk_chats_users_owner_id",
                table: "chats",
                column: "owner_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "fk_chats_users_owner_id", table: "chats");

            migrationBuilder.DropIndex(name: "ix_chats_owner_id", table: "chats");

            migrationBuilder.DropColumn(name: "owner_id", table: "chats");
        }
    }
}
