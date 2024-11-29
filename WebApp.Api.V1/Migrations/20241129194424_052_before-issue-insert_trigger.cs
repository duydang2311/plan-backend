using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Host.Migrations
{
    /// <inheritdoc />
    public partial class _052_beforeissueinsert_trigger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                    create or replace function before_issue_insert() returns trigger as $$
                    begin
                        new.order_number = coalesce(next_shared_counter(new.project_id), 1);
                        return new;
                    end;
                    $$ language plpgsql;
                """
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // 049
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
        }
    }
}
