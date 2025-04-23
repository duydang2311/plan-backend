using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Host.Migrations
{
    /// <inheritdoc />
    public partial class _083_workspaceresources_fixpk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(name: "pk_workspace_resources", table: "workspace_resources");

            migrationBuilder.DropIndex(name: "ix_workspace_resources_resource_id", table: "workspace_resources");

            migrationBuilder.AddPrimaryKey(
                name: "pk_workspace_resources",
                table: "workspace_resources",
                column: "resource_id"
            );

            migrationBuilder.CreateIndex(
                name: "ix_workspace_resources_workspace_id",
                table: "workspace_resources",
                column: "workspace_id"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(name: "pk_workspace_resources", table: "workspace_resources");

            migrationBuilder.DropIndex(name: "ix_workspace_resources_workspace_id", table: "workspace_resources");

            migrationBuilder.AddPrimaryKey(
                name: "pk_workspace_resources",
                table: "workspace_resources",
                column: "workspace_id"
            );

            migrationBuilder.CreateIndex(
                name: "ix_workspace_resources_resource_id",
                table: "workspace_resources",
                column: "resource_id",
                unique: true
            );
        }
    }
}
