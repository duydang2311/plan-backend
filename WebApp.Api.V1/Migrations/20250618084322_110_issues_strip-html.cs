using Microsoft.EntityFrameworkCore.Migrations;
using NpgsqlTypes;

#nullable disable

namespace WebApp.Host.Migrations
{
    /// <inheritdoc />
    public partial class _110_issues_striphtml : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder
                .AlterColumn<NpgsqlTsVector>(
                    name: "search_vector",
                    table: "issues",
                    type: "tsvector",
                    nullable: false,
                    computedColumnSql: "to_tsvector('simple_unaccented', \"title\" || ' ' || regexp_replace(coalesce(\"description\", ''), '<[^>]*>', ' ', 'g'))",
                    stored: true,
                    oldClrType: typeof(NpgsqlTsVector),
                    oldType: "tsvector"
                )
                .OldAnnotation("Npgsql:TsVectorConfig", "simple_unaccented")
                .OldAnnotation("Npgsql:TsVectorProperties", new[] { "title", "description" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder
                .AlterColumn<NpgsqlTsVector>(
                    name: "search_vector",
                    table: "issues",
                    type: "tsvector",
                    nullable: false,
                    oldClrType: typeof(NpgsqlTsVector),
                    oldType: "tsvector",
                    oldComputedColumnSql: "to_tsvector('simple_unaccented', \"title\" || ' ' || regexp_replace(coalesce(\"description\", ''), '<[^>]*>', ' ', 'g'))"
                )
                .Annotation("Npgsql:TsVectorConfig", "simple_unaccented")
                .Annotation("Npgsql:TsVectorProperties", new[] { "title", "description" });
        }
    }
}
