using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Host.Migrations
{
    /// <inheritdoc />
    public partial class _046_userprofiles_imageasset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "image_url",
                table: "user_profiles");

            migrationBuilder.AddColumn<string>(
                name: "image_format",
                table: "user_profiles",
                type: "character varying(16)",
                maxLength: 16,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "image_public_id",
                table: "user_profiles",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "image_resource_type",
                table: "user_profiles",
                type: "character varying(16)",
                maxLength: 16,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "image_version",
                table: "user_profiles",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "image_format",
                table: "user_profiles");

            migrationBuilder.DropColumn(
                name: "image_public_id",
                table: "user_profiles");

            migrationBuilder.DropColumn(
                name: "image_resource_type",
                table: "user_profiles");

            migrationBuilder.DropColumn(
                name: "image_version",
                table: "user_profiles");

            migrationBuilder.AddColumn<string>(
                name: "image_url",
                table: "user_profiles",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true);
        }
    }
}
