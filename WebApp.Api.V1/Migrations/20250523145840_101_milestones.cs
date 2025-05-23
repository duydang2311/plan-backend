using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebApp.Host.Migrations
{
    /// <inheritdoc />
    public partial class _101_milestones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(name: "milestone_id", table: "issues", type: "bigint", nullable: true);

            migrationBuilder.CreateTable(
                name: "milestones",
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
                    project_id = table.Column<Guid>(type: "uuid", nullable: false),
                    end_time = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    title = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    preview_description = table.Column<string>(
                        type: "character varying(256)",
                        maxLength: 256,
                        nullable: true
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_milestones", x => x.id);
                    table.ForeignKey(
                        name: "fk_milestones_projects_project_id",
                        column: x => x.project_id,
                        principalTable: "projects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(name: "ix_issues_milestone_id", table: "issues", column: "milestone_id");

            migrationBuilder.CreateIndex(name: "ix_milestones_project_id", table: "milestones", column: "project_id");

            migrationBuilder.AddForeignKey(
                name: "fk_issues_milestones_milestone_id",
                table: "issues",
                column: "milestone_id",
                principalTable: "milestones",
                principalColumn: "id"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "fk_issues_milestones_milestone_id", table: "issues");

            migrationBuilder.DropTable(name: "milestones");

            migrationBuilder.DropIndex(name: "ix_issues_milestone_id", table: "issues");

            migrationBuilder.DropColumn(name: "milestone_id", table: "issues");
        }
    }
}
