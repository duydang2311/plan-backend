using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebApp.Host.Migrations
{
    /// <inheritdoc />
    public partial class _095_checklistitems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "issue_links");

            migrationBuilder.DropTable(name: "issue_timelines");

            migrationBuilder.AddColumn<Instant>(
                name: "end_time",
                table: "issues",
                type: "timestamp with time zone",
                nullable: true
            );

            migrationBuilder.AddColumn<Instant>(
                name: "start_time",
                table: "issues",
                type: "timestamp with time zone",
                nullable: true
            );

            migrationBuilder.CreateTable(
                name: "checklist_items",
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
                    parent_issue_id = table.Column<Guid>(type: "uuid", nullable: false),
                    kind = table.Column<int>(type: "integer", nullable: false),
                    content = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    sub_issue_id = table.Column<Guid>(type: "uuid", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_checklist_items", x => x.id);
                    table.ForeignKey(
                        name: "fk_checklist_items_issues_parent_issue_id",
                        column: x => x.parent_issue_id,
                        principalTable: "issues",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "fk_checklist_items_issues_sub_issue_id",
                        column: x => x.sub_issue_id,
                        principalTable: "issues",
                        principalColumn: "id"
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "ix_checklist_items_parent_issue_id",
                table: "checklist_items",
                column: "parent_issue_id"
            );

            migrationBuilder.CreateIndex(
                name: "ix_checklist_items_sub_issue_id",
                table: "checklist_items",
                column: "sub_issue_id"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "checklist_items");

            migrationBuilder.DropColumn(name: "end_time", table: "issues");

            migrationBuilder.DropColumn(name: "start_time", table: "issues");

            migrationBuilder.CreateTable(
                name: "issue_links",
                columns: table => new
                {
                    issue_id = table.Column<Guid>(type: "uuid", nullable: false),
                    sub_issue_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_time = table.Column<Instant>(
                        type: "timestamp with time zone",
                        nullable: false,
                        defaultValueSql: "now()"
                    ),
                    id = table
                        .Column<long>(type: "bigint", nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                        ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_issue_links", x => x.issue_id);
                    table.ForeignKey(
                        name: "fk_issue_links_issues_issue_id",
                        column: x => x.issue_id,
                        principalTable: "issues",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "fk_issue_links_issues_sub_issue_id",
                        column: x => x.sub_issue_id,
                        principalTable: "issues",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "issue_timelines",
                columns: table => new
                {
                    issue_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_time = table.Column<Instant>(
                        type: "timestamp with time zone",
                        nullable: false,
                        defaultValueSql: "now()"
                    ),
                    end_time = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    start_time = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    updated_time = table.Column<Instant>(
                        type: "timestamp with time zone",
                        nullable: false,
                        defaultValueSql: "now()"
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_issue_timelines", x => x.issue_id);
                    table.ForeignKey(
                        name: "fk_issue_timelines_issues_issue_id",
                        column: x => x.issue_id,
                        principalTable: "issues",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "ix_issue_links_sub_issue_id",
                table: "issue_links",
                column: "sub_issue_id"
            );
        }
    }
}
