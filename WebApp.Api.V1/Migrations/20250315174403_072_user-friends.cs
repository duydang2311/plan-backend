using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

#nullable disable

namespace WebApp.Host.Migrations
{
    /// <inheritdoc />
    public partial class _072_userfriends : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "user_friend_requests",
                columns: table => new
                {
                    sender_id = table.Column<Guid>(type: "uuid", nullable: false),
                    receiver_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_time = table.Column<Instant>(
                        type: "timestamp with time zone",
                        nullable: false,
                        defaultValueSql: "now()"
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_friend_requests", x => new { x.sender_id, x.receiver_id });
                    table.CheckConstraint("CHK_user_friend_requests_sender_id_receiver_id", "sender_id < receiver_id");
                    table.ForeignKey(
                        name: "fk_user_friend_requests_users_receiver_id",
                        column: x => x.receiver_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "fk_user_friend_requests_users_sender_id",
                        column: x => x.sender_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "user_friends",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    friend_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_time = table.Column<Instant>(
                        type: "timestamp with time zone",
                        nullable: false,
                        defaultValueSql: "now()"
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_friends", x => new { x.user_id, x.friend_id });
                    table.CheckConstraint("CHK_user_friends_user_id_friend_id", "user_id < friend_id");
                    table.ForeignKey(
                        name: "fk_user_friends_users_friend_id",
                        column: x => x.friend_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "fk_user_friends_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "ix_user_friend_requests_receiver_id",
                table: "user_friend_requests",
                column: "receiver_id"
            );

            migrationBuilder.CreateIndex(name: "ix_user_friends_friend_id", table: "user_friends", column: "friend_id");

            migrationBuilder.Sql(
                """
                create function ensure_user_friends_ids()
                returns trigger as $$
                declare temp_id uuid;
                begin
                    if new.user_id > new.friend_id then
                        temp_id := new.user_id;
                        new.user_id := new.friend_id;
                        new.friend_id := temp_id;
                    end if;
                    return new;
                end;
                $$ language plpgsql;

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

                create trigger BI_user_friends before insert on user_friends for each row execute function ensure_user_friends_ids();
                create trigger BI_user_friend_requests before insert on user_friend_requests for each row execute function ensure_user_friend_requests_ids();
                """
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                drop trigger BI_user_friends on user_friends;
                drop function ensure_user_friends_ids();
                drop trigger BI_user_friend_requests on user_friend_requests;
                drop function ensure_user_friend_requests_ids();
                """
            );

            migrationBuilder.DropTable(name: "user_friend_requests");

            migrationBuilder.DropTable(name: "user_friends");
        }
    }
}
