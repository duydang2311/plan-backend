using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebApp.Host.Migrations
{
    /// <inheritdoc />
    public partial class _059_casbin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "policies");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "policies",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ptype = table.Column<string>(type: "text", nullable: true),
                    v0 = table.Column<string>(type: "text", nullable: true),
                    v1 = table.Column<string>(type: "text", nullable: true),
                    v2 = table.Column<string>(type: "text", nullable: true),
                    v3 = table.Column<string>(type: "text", nullable: true),
                    v4 = table.Column<string>(type: "text", nullable: true),
                    v5 = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_policies", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_policies_ptype",
                table: "policies",
                column: "ptype");

            migrationBuilder.CreateIndex(
                name: "ix_policies_v0",
                table: "policies",
                column: "v0");

            migrationBuilder.CreateIndex(
                name: "ix_policies_v1",
                table: "policies",
                column: "v1");

            migrationBuilder.CreateIndex(
                name: "ix_policies_v2",
                table: "policies",
                column: "v2");

            migrationBuilder.CreateIndex(
                name: "ix_policies_v3",
                table: "policies",
                column: "v3");

            migrationBuilder.CreateIndex(
                name: "ix_policies_v4",
                table: "policies",
                column: "v4");

            migrationBuilder.CreateIndex(
                name: "ix_policies_v5",
                table: "policies",
                column: "v5");
        }
    }
}
