using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Host.Migrations
{
    /// <inheritdoc />
    public partial class _075_users_userprofiles_search : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(name: "ix_users_email", table: "users");

            migrationBuilder.AddColumn<string>(
                name: "trigrams",
                table: "users",
                type: "text",
                nullable: false,
                computedColumnSql: "\"email\"",
                stored: true
            );

            migrationBuilder.AddColumn<string>(
                name: "trigrams",
                table: "user_profiles",
                type: "text",
                nullable: false,
                computedColumnSql: "\"name\" || ' ' || \"display_name\"",
                stored: true
            );

            migrationBuilder.CreateIndex(name: "ix_users_email", table: "users", column: "email");

            migrationBuilder
                .CreateIndex(name: "ix_users_trigrams", table: "users", column: "trigrams")
                .Annotation("Npgsql:IndexMethod", "gin")
                .Annotation("Npgsql:IndexOperators", new[] { "gin_trgm_ops" });

            migrationBuilder
                .CreateIndex(name: "ix_user_profiles_trigrams", table: "user_profiles", column: "trigrams")
                .Annotation("Npgsql:IndexMethod", "gin")
                .Annotation("Npgsql:IndexOperators", new[] { "gin_trgm_ops" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(name: "ix_users_email", table: "users");

            migrationBuilder.DropIndex(name: "ix_users_trigrams", table: "users");

            migrationBuilder.DropIndex(name: "ix_user_profiles_trigrams", table: "user_profiles");

            migrationBuilder.DropColumn(name: "trigrams", table: "users");

            migrationBuilder.DropColumn(name: "trigrams", table: "user_profiles");

            migrationBuilder
                .CreateIndex(name: "ix_users_email", table: "users", column: "email")
                .Annotation("Npgsql:IndexMethod", "gin")
                .Annotation("Npgsql:IndexOperators", new[] { "gin_trgm_ops" });
        }
    }
}
