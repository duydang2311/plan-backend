using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Host.Migrations
{
    /// <inheritdoc />
    public partial class _014_shared_counters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "shared_counters",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    count = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_shared_counters", x => x.id);
                }
            );

            migrationBuilder.Sql("""
                create function next_shared_counter(target_id uuid) returns bigint as $$
                declare
                    ret bigint;
                begin
                    update shared_counters set count = count + 1 where id = target_id returning count into ret;
                    return ret;
                end;
                $$ language plpgsql;
            """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                drop function next_shared_counter
            """);
            migrationBuilder.DropTable("shared_counters");
        }
    }
}
