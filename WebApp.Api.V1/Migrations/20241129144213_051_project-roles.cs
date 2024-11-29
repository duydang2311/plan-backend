using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;
using WebApp.Domain.Constants;

#nullable disable

namespace WebApp.Host.Migrations
{
    /// <inheritdoc />
    public partial class _051_projectroles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "fk_issues_teams_team_id", table: "issues");

            migrationBuilder.DropTable(name: "project_issues");

            migrationBuilder.RenameColumn(name: "team_id", table: "issues", newName: "project_id");

            migrationBuilder.RenameIndex(
                name: "ix_issues_team_id_order_number",
                table: "issues",
                newName: "ix_issues_project_id_order_number"
            );

            migrationBuilder.AddColumn<Guid>(name: "project_id", table: "user_roles", type: "uuid", nullable: true);

            migrationBuilder.CreateTable(
                name: "team_issues",
                columns: table => new
                {
                    team_id = table.Column<Guid>(type: "uuid", nullable: false),
                    issue_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_time = table.Column<Instant>(
                        type: "timestamp with time zone",
                        nullable: false,
                        defaultValueSql: "now()"
                    ),
                    rank = table.Column<string>(type: "text", nullable: false, collation: "C")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_team_issues", x => new { x.team_id, x.issue_id });
                    table.ForeignKey(
                        name: "fk_team_issues_issues_issue_id",
                        column: x => x.issue_id,
                        principalTable: "issues",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "fk_team_issues_teams_team_id",
                        column: x => x.team_id,
                        principalTable: "teams",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(name: "ix_user_roles_project_id", table: "user_roles", column: "project_id");

            migrationBuilder.CreateIndex(name: "ix_team_issues_issue_id", table: "team_issues", column: "issue_id");

            migrationBuilder.AddForeignKey(
                name: "fk_issues_projects_project_id",
                table: "issues",
                column: "project_id",
                principalTable: "projects",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "fk_user_roles_projects_project_id",
                table: "user_roles",
                column: "project_id",
                principalTable: "projects",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );

            foreach (var role in ProjectRoleDefaults.Roles)
            {
                migrationBuilder.InsertData(
                    table: "roles",
                    columns: ["id", "name"],
                    values: [role.Id.Value, role.Name]
                );
                foreach (var permission in role.Permissions)
                {
                    migrationBuilder.InsertData(
                        table: "role_permissions",
                        columns: ["role_id", "permission"],
                        values: [role.Id, permission]
                    );
                }
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "fk_issues_projects_project_id", table: "issues");

            migrationBuilder.DropForeignKey(name: "fk_user_roles_projects_project_id", table: "user_roles");

            migrationBuilder.DropTable(name: "team_issues");

            migrationBuilder.DropIndex(name: "ix_user_roles_project_id", table: "user_roles");

            migrationBuilder.DropColumn(name: "project_id", table: "user_roles");

            migrationBuilder.RenameColumn(name: "project_id", table: "issues", newName: "team_id");

            migrationBuilder.RenameIndex(
                name: "ix_issues_project_id_order_number",
                table: "issues",
                newName: "ix_issues_team_id_order_number"
            );

            migrationBuilder.CreateTable(
                name: "project_issues",
                columns: table => new
                {
                    project_id = table.Column<Guid>(type: "uuid", nullable: false),
                    issue_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_time = table.Column<Instant>(
                        type: "timestamp with time zone",
                        nullable: false,
                        defaultValueSql: "now()"
                    ),
                    rank = table.Column<string>(type: "text", nullable: false, collation: "C")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_project_issues", x => new { x.project_id, x.issue_id });
                    table.ForeignKey(
                        name: "fk_project_issues_issues_issue_id",
                        column: x => x.issue_id,
                        principalTable: "issues",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "fk_project_issues_projects_project_id",
                        column: x => x.project_id,
                        principalTable: "projects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "ix_project_issues_issue_id",
                table: "project_issues",
                column: "issue_id"
            );

            migrationBuilder.AddForeignKey(
                name: "fk_issues_teams_team_id",
                table: "issues",
                column: "team_id",
                principalTable: "teams",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );
        }
    }
}
