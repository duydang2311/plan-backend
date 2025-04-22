using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebApp.Host.Migrations
{
    /// <inheritdoc />
    public partial class _080_resources : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Instant>(
                name: "deleted_time",
                table: "workspaces",
                type: "timestamp with time zone",
                nullable: true
            );

            migrationBuilder.AddColumn<Instant>(
                name: "deleted_time",
                table: "projects",
                type: "timestamp with time zone",
                nullable: true
            );

            migrationBuilder.CreateTable(
                name: "resources",
                columns: table => new
                {
                    id = table
                        .Column<long>(type: "bigint", nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                        ),
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
                    type = table.Column<byte>(type: "smallint", nullable: false),
                    creator_id = table.Column<Guid>(type: "uuid", nullable: false),
                    content = table.Column<string>(type: "text", nullable: true),
                    key = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_resources", x => x.id);
                    table.ForeignKey(
                        name: "fk_resources_users_creator_id",
                        column: x => x.creator_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "project_resources",
                columns: table => new
                {
                    resource_id = table.Column<long>(type: "bigint", nullable: false),
                    project_id = table.Column<Guid>(type: "uuid", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_project_resources", x => x.resource_id);
                    table.ForeignKey(
                        name: "fk_project_resources_projects_project_id",
                        column: x => x.project_id,
                        principalTable: "projects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "fk_project_resources_resources_resource_id",
                        column: x => x.resource_id,
                        principalTable: "resources",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "workspace_resources",
                columns: table => new
                {
                    workspace_id = table.Column<Guid>(type: "uuid", nullable: false),
                    resource_id = table.Column<long>(type: "bigint", nullable: false),
                    workspace_id1 = table.Column<Guid>(type: "uuid", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_workspace_resources", x => x.workspace_id);
                    table.ForeignKey(
                        name: "fk_workspace_resources_resources_resource_id",
                        column: x => x.resource_id,
                        principalTable: "resources",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "fk_workspace_resources_workspaces_workspace_id1",
                        column: x => x.workspace_id1,
                        principalTable: "workspaces",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "ix_workspaces_deleted_time",
                table: "workspaces",
                column: "deleted_time"
            );

            migrationBuilder.CreateIndex(name: "ix_projects_deleted_time", table: "projects", column: "deleted_time");

            migrationBuilder.CreateIndex(
                name: "ix_project_resources_project_id",
                table: "project_resources",
                column: "project_id"
            );

            migrationBuilder.CreateIndex(name: "ix_resources_creator_id", table: "resources", column: "creator_id");

            migrationBuilder.CreateIndex(
                name: "ix_workspace_resources_resource_id",
                table: "workspace_resources",
                column: "resource_id",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "ix_workspace_resources_workspace_id1",
                table: "workspace_resources",
                column: "workspace_id1"
            );

            migrationBuilder.Sql(
                """
                    create or replace function update_resource_updated_time() returns trigger as $$
                        begin
                            new.updated_time = NOW();
                            return new;
                        end;
                    $$ language plpgsql;

                    create trigger TR_resources_update_resource_updated_time
                    before update on resources
                    for each row 
                    execute function update_resource_updated_time();
                """
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                    drop trigger TR_resources_update_resource_updated_time on resources;
                    drop function update_resource_updated_time();
                """
            );
            migrationBuilder.DropTable(name: "project_resources");

            migrationBuilder.DropTable(name: "workspace_resources");

            migrationBuilder.DropTable(name: "resources");

            migrationBuilder.DropIndex(name: "ix_workspaces_deleted_time", table: "workspaces");

            migrationBuilder.DropIndex(name: "ix_projects_deleted_time", table: "projects");

            migrationBuilder.DropColumn(name: "deleted_time", table: "workspaces");

            migrationBuilder.DropColumn(name: "deleted_time", table: "projects");
        }
    }
}
