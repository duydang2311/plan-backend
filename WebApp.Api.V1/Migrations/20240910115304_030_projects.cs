using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

#nullable disable

namespace WebApp.Host.Migrations
{
    /// <inheritdoc />
    public partial class _030_projects : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "projects",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_time = table.Column<Instant>(
                        type: "timestamp with time zone",
                        nullable: false,
                        defaultValueSql: "now()"
                    ),
                    updated_time = table.Column<Instant>(
                        type: "timestamp with time zone",
                        nullable: false,
                        defaultValueSql: "now()"
                    ),
                    workspace_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    identifier = table.Column<string>(
                        type: "character varying(64)",
                        maxLength: 64,
                        nullable: false,
                        collation: "case_insensitive"
                    ),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_projects", x => x.id);
                    table.ForeignKey(
                        name: "fk_projects_workspaces_workspace_id",
                        column: x => x.workspace_id,
                        principalTable: "workspaces",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "project_statuses",
                columns: table => new
                {
                    status_id = table.Column<long>(type: "bigint", nullable: false),
                    project_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_project_statuses", x => x.status_id);
                    table.ForeignKey(
                        name: "fk_project_statuses_projects_project_id",
                        column: x => x.project_id,
                        principalTable: "projects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "fk_project_statuses_statuses_status_id",
                        column: x => x.status_id,
                        principalTable: "statuses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "ix_project_statuses_project_id",
                table: "project_statuses",
                column: "project_id"
            );

            migrationBuilder.CreateIndex(
                name: "ix_projects_workspace_id_identifier",
                table: "projects",
                columns: new[] { "workspace_id", "identifier" },
                unique: true
            );

            migrationBuilder.Sql(
                """
                    create function update_project() returns trigger as $$
                    begin
                        new.updated_time := now();
                        return new;
                    end;
                    $$ language plpgsql;

                    create trigger TR_projects_BU before update on projects
                    for each row execute function update_project();
                """
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                    drop trigger TR_projects_BU on projects;
                    drop function update_project();
                """
            );

            migrationBuilder.DropTable(name: "project_statuses");

            migrationBuilder.DropTable(name: "projects");
        }
    }
}
