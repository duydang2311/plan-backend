using Microsoft.EntityFrameworkCore.Migrations;
using WebApp.Domain.Constants;

#nullable disable

namespace WebApp.Host.Migrations
{
    /// <inheritdoc />
    public partial class _018_teamroles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "role_id",
                table: "team_members",
                type: "integer",
                nullable: false,
                defaultValue: 0
            );

            migrationBuilder.CreateTable(
                name: "team_roles",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_team_roles", x => x.id);
                }
            );

            migrationBuilder.CreateTable(
                name: "team_role_permissions",
                columns: table => new
                {
                    role_id = table.Column<int>(type: "integer", nullable: false),
                    permission = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_team_role_permissions", x => new { x.role_id, x.permission });
                    table.ForeignKey(
                        name: "fk_team_role_permissions_team_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "team_roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(name: "ix_team_members_role_id", table: "team_members", column: "role_id");

            migrationBuilder.CreateIndex(
                name: "ix_team_role_permissions_role_id",
                table: "team_role_permissions",
                column: "role_id"
            );

            migrationBuilder.AddForeignKey(
                name: "fk_team_members_team_roles_role_id",
                table: "team_members",
                column: "role_id",
                principalTable: "team_roles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );

            foreach (var role in TeamRoleDefaults.Roles)
            {
                migrationBuilder.InsertData(
                    table: "team_roles",
                    columns: ["id", "name"],
                    values: [role.Id.Value, role.Name]
                );
                foreach (var permission in role.Permissions)
                {
                    migrationBuilder.InsertData(
                        table: "team_role_permissions",
                        columns: ["role_id", "permission"],
                        values: [role.Id, permission]
                    );
                }
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "fk_team_members_team_roles_role_id", table: "team_members");

            migrationBuilder.DropTable(name: "team_role_permissions");

            migrationBuilder.DropTable(name: "team_roles");

            migrationBuilder.DropIndex(name: "ix_team_members_role_id", table: "team_members");

            migrationBuilder.DropColumn(name: "role_id", table: "team_members");
        }
    }
}
