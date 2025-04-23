using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Host.Migrations
{
    /// <inheritdoc />
    public partial class _084_resources_namerank : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "resources",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: ""
            );

            migrationBuilder.AddColumn<string>(
                name: "rank",
                table: "resources",
                type: "text",
                nullable: false,
                defaultValue: "",
                collation: "C"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "name", table: "resources");

            migrationBuilder.DropColumn(name: "rank", table: "resources");
        }
    }
}
