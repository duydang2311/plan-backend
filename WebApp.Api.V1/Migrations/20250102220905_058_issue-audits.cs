using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebApp.Host.Migrations
{
    /// <inheritdoc />
    public partial class _058_issueaudits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "issue_audits",
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
                    issue_id = table.Column<Guid>(type: "uuid", nullable: false),
                    action = table.Column<byte>(type: "smallint", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    data = table.Column<JsonDocument>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_issue_audits", x => x.id);
                    table.ForeignKey(
                        name: "fk_issue_audits_issues_issue_id",
                        column: x => x.issue_id,
                        principalTable: "issues",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "fk_issue_audits_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id"
                    );
                }
            );

            migrationBuilder.CreateIndex(name: "ix_issue_audits_issue_id", table: "issue_audits", column: "issue_id");

            migrationBuilder.CreateIndex(name: "ix_issue_audits_user_id", table: "issue_audits", column: "user_id");

            migrationBuilder.Sql(
                """
                    create function create_issue_audits_action_create() returns trigger as $$
                    begin
                        insert into issue_audits(issue_id, user_id) values (NEW.id, NEW.author_id);
                        return null;
                    end;
                    $$ language plpgsql;

                    create trigger TR_issues_AI_issue_audits after insert on issues
                    for each row execute function create_issue_audits_action_create();
                """
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                    drop trigger TR_issues_AI_issue_audits on issues;
                    drop function create_issue_audits_action_create();
                """
            );
            migrationBuilder.DropTable(name: "issue_audits");
        }
    }
}
