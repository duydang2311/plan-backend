using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebApp.Host.Migrations
{
    /// <inheritdoc />
    public partial class _081_storagependinguploads : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "storage_pending_uploads",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_time = table.Column<Instant>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    key = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    expiry_time = table.Column<Instant>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_storage_pending_uploads", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_storage_pending_uploads_expiry_time",
                table: "storage_pending_uploads",
                column: "expiry_time");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "storage_pending_uploads");
        }
    }
}
