using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Host.Migrations
{
    /// <inheritdoc />
    public partial class _087_usersessions_sessionid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(name: "pk_user_sessions", table: "user_sessions");

            migrationBuilder.DropColumn(name: "token", table: "user_sessions");

            migrationBuilder.AddColumn<string>(
                name: "session_id",
                table: "user_sessions",
                type: "character varying(22)",
                maxLength: 22,
                nullable: false,
                defaultValue: ""
            );

            migrationBuilder.AddPrimaryKey(name: "pk_user_sessions", table: "user_sessions", column: "session_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(name: "pk_user_sessions", table: "user_sessions");

            migrationBuilder.DropColumn(name: "session_id", table: "user_sessions");

            migrationBuilder.AddColumn<Guid>(
                name: "token",
                table: "user_sessions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000")
            );

            migrationBuilder.AddPrimaryKey(name: "pk_user_sessions", table: "user_sessions", column: "token");
        }
    }
}
