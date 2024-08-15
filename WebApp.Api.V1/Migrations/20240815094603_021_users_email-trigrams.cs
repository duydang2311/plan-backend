using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Host.Migrations
{
    /// <inheritdoc />
    public partial class _021_users_emailtrigrams : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("CREATE EXTENSION pg_trgm");

            migrationBuilder.DropIndex(name: "ix_users_email", table: "users");

            migrationBuilder
                .CreateIndex(name: "ix_users_email", table: "users", column: "email")
                .Annotation("Npgsql:IndexMethod", "gin")
                .Annotation("Npgsql:IndexOperators", new[] { "gin_trgm_ops" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(name: "ix_users_email", table: "users");

            migrationBuilder.CreateIndex(name: "ix_users_email", table: "users", column: "email", unique: true);
        }
    }
}
