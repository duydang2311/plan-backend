using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebApp.Host.Migrations
{
    /// <inheritdoc />
    public partial class _038_statuses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "fk_issues_statuses_status_id", table: "issues");

            migrationBuilder.DropForeignKey(
                name: "fk_workspace_statuses_statuses_status_id",
                table: "workspace_statuses"
            );

            migrationBuilder.DropTable(name: "project_statuses");

            migrationBuilder.DropTable(name: "statuses");

            migrationBuilder.DropPrimaryKey(name: "pk_workspace_statuses", table: "workspace_statuses");

            migrationBuilder.DropColumn(name: "status_id", table: "workspace_statuses");

            migrationBuilder.CreateSequence(name: "StatusSequence");

            migrationBuilder.AddColumn<long>(
                name: "id",
                table: "workspace_statuses",
                type: "bigint",
                nullable: false,
                defaultValueSql: "nextval('\"StatusSequence\"')"
            );

            migrationBuilder.AddColumn<string>(
                name: "color",
                table: "workspace_statuses",
                type: "character varying(16)",
                maxLength: 16,
                nullable: false,
                defaultValue: ""
            );

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "workspace_statuses",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true
            );

            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                table: "workspace_statuses",
                type: "boolean",
                nullable: false,
                defaultValue: false
            );

            migrationBuilder.AddColumn<int>(
                name: "rank",
                table: "workspace_statuses",
                type: "integer",
                nullable: false,
                defaultValue: 0
            );

            migrationBuilder.AddColumn<string>(
                name: "value",
                table: "workspace_statuses",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: ""
            );

            migrationBuilder.AddPrimaryKey(name: "PK_workspace_statuses", table: "workspace_statuses", column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_issues_workspace_statuss_status_id",
                table: "issues",
                column: "status_id",
                principalTable: "workspace_statuses",
                principalColumn: "id"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "fk_issues_workspace_statuss_status_id", table: "issues");

            migrationBuilder.DropPrimaryKey(name: "PK_workspace_statuses", table: "workspace_statuses");

            migrationBuilder.DropColumn(name: "id", table: "workspace_statuses");

            migrationBuilder.DropColumn(name: "color", table: "workspace_statuses");

            migrationBuilder.DropColumn(name: "description", table: "workspace_statuses");

            migrationBuilder.DropColumn(name: "is_default", table: "workspace_statuses");

            migrationBuilder.DropColumn(name: "rank", table: "workspace_statuses");

            migrationBuilder.DropColumn(name: "value", table: "workspace_statuses");

            migrationBuilder.DropSequence(name: "StatusSequence");

            migrationBuilder.AddColumn<long>(
                name: "status_id",
                table: "workspace_statuses",
                type: "bigint",
                nullable: false,
                defaultValue: 0L
            );

            migrationBuilder.AddPrimaryKey(
                name: "pk_workspace_statuses",
                table: "workspace_statuses",
                column: "status_id"
            );

            migrationBuilder.CreateTable(
                name: "statuses",
                columns: table => new
                {
                    id = table
                        .Column<long>(type: "bigint", nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                        ),
                    color = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    rank = table.Column<int>(type: "integer", nullable: false),
                    value = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_statuses", x => x.id);
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

            migrationBuilder.AddForeignKey(
                name: "fk_issues_statuses_status_id",
                table: "issues",
                column: "status_id",
                principalTable: "statuses",
                principalColumn: "id"
            );

            migrationBuilder.AddForeignKey(
                name: "fk_workspace_statuses_statuses_status_id",
                table: "workspace_statuses",
                column: "status_id",
                principalTable: "statuses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );
        }
    }
}
