using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Host.Migrations
{
    /// <inheritdoc />
    public partial class _028_issues_orderbystatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "order_by_status",
                table: "issues",
                type: "bigint",
                nullable: false,
                defaultValue: 0L
            );

            migrationBuilder.Sql(
                """
                    create or replace function before_issue_insert() returns trigger as $$
                    begin
                        new.order_number = coalesce(next_shared_counter(new.team_id), 1);
                        select coalesce(max(order_by_status) + 1, 0) into new.order_by_status from issues where team_id = new.team_id and status_id is not distinct from new.status_id;
                        return new;
                    end;
                    $$ language plpgsql;
                """
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // 015
            migrationBuilder.Sql(
                """
                    create or replace function before_issue_insert() returns trigger as $$
                    begin
                        new.order_number = coalesce(next_shared_counter(new.team_id), 1);
                        return new;
                    end;
                    $$ language plpgsql;
                """
            );
            migrationBuilder.DropColumn(name: "order_by_status", table: "issues");
        }
    }
}
