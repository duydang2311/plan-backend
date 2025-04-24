using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebApp.Host.Migrations
{
    /// <inheritdoc />
    public partial class _085_resources : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "content", table: "resources");

            migrationBuilder.DropColumn(name: "key", table: "resources");

            migrationBuilder.DropColumn(name: "type", table: "resources");

            migrationBuilder.CreateTable(
                name: "resource_documents",
                columns: table => new
                {
                    resource_id = table.Column<long>(type: "bigint", nullable: false),
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
                    content = table.Column<string>(type: "text", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_resource_documents", x => x.resource_id);
                    table.ForeignKey(
                        name: "fk_resource_documents_resources_resource_id",
                        column: x => x.resource_id,
                        principalTable: "resources",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "resource_files",
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
                    resource_id = table.Column<long>(type: "bigint", nullable: false),
                    key = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_resource_files", x => x.id);
                    table.ForeignKey(
                        name: "fk_resource_files_resources_resource_id",
                        column: x => x.resource_id,
                        principalTable: "resources",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "ix_resource_files_resource_id",
                table: "resource_files",
                column: "resource_id"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "resource_documents");

            migrationBuilder.DropTable(name: "resource_files");

            migrationBuilder.AddColumn<string>(name: "content", table: "resources", type: "text", nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "key",
                table: "resources",
                type: "character varying(1024)",
                maxLength: 1024,
                nullable: true
            );

            migrationBuilder.AddColumn<byte>(
                name: "type",
                table: "resources",
                type: "smallint",
                nullable: false,
                defaultValue: (byte)0
            );
        }
    }
}
