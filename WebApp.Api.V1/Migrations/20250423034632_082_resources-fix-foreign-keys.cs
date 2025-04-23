using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Host.Migrations
{
    /// <inheritdoc />
    public partial class _082_resourcesfixforeignkeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_workspace_resources_workspaces_workspace_id1",
                table: "workspace_resources"
            );

            migrationBuilder.DropIndex(name: "ix_workspace_resources_workspace_id1", table: "workspace_resources");

            migrationBuilder.DropColumn(name: "workspace_id1", table: "workspace_resources");

            migrationBuilder.AddForeignKey(
                name: "fk_workspace_resources_workspaces_workspace_id",
                table: "workspace_resources",
                column: "workspace_id",
                principalTable: "workspaces",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_workspace_resources_workspaces_workspace_id",
                table: "workspace_resources"
            );

            migrationBuilder.AddColumn<Guid>(
                name: "workspace_id1",
                table: "workspace_resources",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000")
            );

            migrationBuilder.CreateIndex(
                name: "ix_workspace_resources_workspace_id1",
                table: "workspace_resources",
                column: "workspace_id1"
            );

            migrationBuilder.AddForeignKey(
                name: "fk_workspace_resources_workspaces_workspace_id1",
                table: "workspace_resources",
                column: "workspace_id1",
                principalTable: "workspaces",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );
        }
    }
}
