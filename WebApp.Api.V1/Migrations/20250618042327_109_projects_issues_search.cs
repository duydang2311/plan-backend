using Microsoft.EntityFrameworkCore.Migrations;
using NpgsqlTypes;

#nullable disable

namespace WebApp.Host.Migrations
{
    /// <inheritdoc />
    public partial class _109_projects_issues_search : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(name: "ix_issues_trigrams", table: "issues");

            migrationBuilder.DropColumn(name: "trigrams", table: "issues");

            migrationBuilder
                .AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:pg_trgm", ",,")
                .Annotation("Npgsql:PostgresExtension:unaccent", ",,");

            migrationBuilder.Sql(
                """
                DO $$
                BEGIN
                    IF NOT EXISTS (SELECT 1 FROM pg_ts_config WHERE cfgname = 'simple_unaccented') THEN
                        CREATE TEXT SEARCH CONFIGURATION simple_unaccented (COPY = simple);
                        ALTER TEXT SEARCH CONFIGURATION simple_unaccented
                            ALTER MAPPING FOR hword, hword_part, word
                            WITH unaccent, simple;
                    END IF;
                END$$;
                """
            );

            migrationBuilder.Sql(
                """
                CREATE OR REPLACE FUNCTION immutable_unaccent(text)
                RETURNS text AS
                $$
                    SELECT public.unaccent('public.unaccent', $1)
                $$ LANGUAGE sql IMMUTABLE;
                """
            );

            migrationBuilder
                .AddColumn<NpgsqlTsVector>(name: "search_vector", table: "projects", type: "tsvector", nullable: false)
                .Annotation("Npgsql:TsVectorConfig", "simple_unaccented")
                .Annotation("Npgsql:TsVectorProperties", new[] { "name", "identifier", "description" });

            migrationBuilder
                .AddColumn<NpgsqlTsVector>(name: "search_vector", table: "issues", type: "tsvector", nullable: false)
                .Annotation("Npgsql:TsVectorConfig", "simple_unaccented")
                .Annotation("Npgsql:TsVectorProperties", new[] { "title", "description" });

            migrationBuilder
                .CreateIndex(name: "ix_projects_search_vector", table: "projects", column: "search_vector")
                .Annotation("Npgsql:IndexMethod", "gin");

            migrationBuilder.Sql(
                "CREATE INDEX \"ix_projects_identifier_name\" ON \"projects\" USING gin(immutable_unaccent(\"identifier\") gin_trgm_ops, immutable_unaccent(\"name\") gin_trgm_ops);"
            );

            migrationBuilder
                .CreateIndex(name: "ix_issues_search_vector", table: "issues", column: "search_vector")
                .Annotation("Npgsql:IndexMethod", "gin");

            migrationBuilder.Sql(
                "CREATE INDEX \"ix_issues_title\" ON \"issues\" USING gin(immutable_unaccent(\"title\") gin_trgm_ops);"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(name: "ix_projects_search_vector", table: "projects");

            migrationBuilder.DropIndex(name: "ix_projects_identifier_name", table: "projects");

            migrationBuilder.DropIndex(name: "ix_issues_search_vector", table: "issues");

            migrationBuilder.DropIndex(name: "ix_issues_title", table: "issues");

            migrationBuilder.DropColumn(name: "search_vector", table: "projects");

            migrationBuilder.DropColumn(name: "search_vector", table: "issues");

            migrationBuilder
                .AlterDatabase()
                .OldAnnotation("Npgsql:PostgresExtension:pg_trgm", ",,")
                .OldAnnotation("Npgsql:PostgresExtension:unaccent", ",,");

            migrationBuilder.AddColumn<string>(
                name: "trigrams",
                table: "issues",
                type: "text",
                nullable: false,
                computedColumnSql: "repeat(\"order_number\"::text || ' ', 4) || \"title\" || ' '",
                stored: true
            );

            migrationBuilder
                .CreateIndex(name: "ix_issues_trigrams", table: "issues", column: "trigrams")
                .Annotation("Npgsql:IndexMethod", "gin")
                .Annotation("Npgsql:IndexOperators", new[] { "gin_trgm_ops" });

            migrationBuilder.Sql("DROP TEXT SEARCH CONFIGURATION simple_unaccented;");
            migrationBuilder.Sql("DROP FUNCTION immutable_unaccent(text);");
        }
    }
}
