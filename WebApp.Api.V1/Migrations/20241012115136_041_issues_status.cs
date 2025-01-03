﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Host.Migrations
{
    /// <inheritdoc />
    public partial class _041_issues_status : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_issues_workspace_statuses_status_id",
                table: "issues");

            migrationBuilder.AddForeignKey(
                name: "fk_issues_workspace_statuses_status_id",
                table: "issues",
                column: "status_id",
                principalTable: "workspace_statuses",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_issues_workspace_statuses_status_id",
                table: "issues");

            migrationBuilder.AddForeignKey(
                name: "fk_issues_workspace_statuses_status_id",
                table: "issues",
                column: "status_id",
                principalTable: "workspace_statuses",
                principalColumn: "id");
        }
    }
}
