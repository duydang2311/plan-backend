using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Host.Migrations
{
    /// <inheritdoc />
    public partial class _015_issues_ordernumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "order_number",
                table: "issues",
                type: "bigint",
                nullable: false);
            migrationBuilder.Sql("""
                create function before_issue_insert() returns trigger as $$
                begin
                    new.order_number = coalesce(next_shared_counter(new.team_id), 1);
                    return new;
                end;
                $$ language plpgsql;

                create trigger TR_issues_BI before insert on issues
                for each row execute function before_issue_insert();
            """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                drop trigger TR_issues_BI on issues;
                drop function before_issue_insert();
            """);
            migrationBuilder.DropColumn(
                name: "order_number",
                table: "issues");
        }
    }
}
