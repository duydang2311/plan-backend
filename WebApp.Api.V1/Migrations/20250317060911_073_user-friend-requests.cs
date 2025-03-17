using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Host.Migrations
{
    /// <inheritdoc />
    public partial class _073_userfriendrequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CHK_user_friend_requests_sender_id_receiver_id",
                table: "user_friend_requests"
            );

            migrationBuilder.Sql(
                """
                drop trigger BI_user_friend_requests on user_friend_requests;
                drop function ensure_user_friend_requests_ids();
                """
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "CHK_user_friend_requests_sender_id_receiver_id",
                table: "user_friend_requests",
                sql: "sender_id < receiver_id"
            );

            migrationBuilder.Sql(
                """
                create function ensure_user_friend_requests_ids()
                returns trigger as $$
                declare temp_id uuid;
                begin
                    if new.sender_id > new.receiver_id then
                        temp_id := new.sender_id;
                        new.sender_id := new.receiver_id;
                        new.receiver_id := temp_id;
                    end if;
                    return new;
                end;
                $$ language plpgsql;

                create trigger BI_user_friend_requests before insert on user_friend_requests for each row execute function ensure_user_friend_requests_ids();
                """
            );
        }
    }
}
