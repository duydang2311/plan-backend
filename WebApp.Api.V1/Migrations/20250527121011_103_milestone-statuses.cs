using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebApp.Host.Migrations
{
    /// <inheritdoc />
    public partial class _103_milestonestatuses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(name: "status_id", table: "milestones", type: "bigint", nullable: true);

            migrationBuilder.CreateTable(
                name: "milestone_statuses",
                columns: table => new
                {
                    id = table
                        .Column<long>(type: "bigint", nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                        ),
                    category = table.Column<byte>(type: "smallint", nullable: false),
                    rank = table.Column<int>(type: "integer", nullable: false),
                    value = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    color = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    icon = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    project_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_milestone_statuses", x => x.id);
                    table.ForeignKey(
                        name: "fk_milestone_statuses_projects_project_id",
                        column: x => x.project_id,
                        principalTable: "projects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(name: "ix_milestones_status_id", table: "milestones", column: "status_id");

            migrationBuilder.CreateIndex(
                name: "ix_milestone_statuses_project_id",
                table: "milestone_statuses",
                column: "project_id"
            );

            migrationBuilder.AddForeignKey(
                name: "fk_milestones_milestone_statuses_status_id",
                table: "milestones",
                column: "status_id",
                principalTable: "milestone_statuses",
                principalColumn: "id"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "fk_milestones_milestone_statuses_status_id", table: "milestones");

            migrationBuilder.DropTable(name: "milestone_statuses");

            migrationBuilder.DropIndex(name: "ix_milestones_status_id", table: "milestones");

            migrationBuilder.DropColumn(name: "status_id", table: "milestones");
        }
    }
}
