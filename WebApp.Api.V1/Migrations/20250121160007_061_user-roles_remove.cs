using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Host.Migrations
{
    /// <inheritdoc />
    public partial class _061_userroles_remove : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_user_roles_projects_project_id",
                table: "user_roles");

            migrationBuilder.DropForeignKey(
                name: "fk_user_roles_roles_role_id",
                table: "user_roles");

            migrationBuilder.DropForeignKey(
                name: "fk_user_roles_users_user_id",
                table: "user_roles");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user_roles",
                table: "user_roles");

            migrationBuilder.DropColumn(
                name: "role_type",
                table: "user_roles");

            migrationBuilder.RenameTable(
                name: "user_roles",
                newName: "project_members");

            migrationBuilder.RenameColumn(
                name: "user_role_id",
                table: "project_members",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "ix_user_roles_user_id",
                table: "project_members",
                newName: "ix_project_members_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_user_roles_role_id",
                table: "project_members",
                newName: "ix_project_members_role_id");

            migrationBuilder.RenameIndex(
                name: "ix_user_roles_project_id",
                table: "project_members",
                newName: "ix_project_members_project_id");

            migrationBuilder.AlterColumn<Guid>(
                name: "project_id",
                table: "project_members",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "pk_project_members",
                table: "project_members",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_project_members_projects_project_id",
                table: "project_members",
                column: "project_id",
                principalTable: "projects",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_project_members_roles_role_id",
                table: "project_members",
                column: "role_id",
                principalTable: "roles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_project_members_users_user_id",
                table: "project_members",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_project_members_projects_project_id",
                table: "project_members");

            migrationBuilder.DropForeignKey(
                name: "fk_project_members_roles_role_id",
                table: "project_members");

            migrationBuilder.DropForeignKey(
                name: "fk_project_members_users_user_id",
                table: "project_members");

            migrationBuilder.DropPrimaryKey(
                name: "pk_project_members",
                table: "project_members");

            migrationBuilder.RenameTable(
                name: "project_members",
                newName: "user_roles");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "user_roles",
                newName: "user_role_id");

            migrationBuilder.RenameIndex(
                name: "ix_project_members_user_id",
                table: "user_roles",
                newName: "ix_user_roles_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_project_members_role_id",
                table: "user_roles",
                newName: "ix_user_roles_role_id");

            migrationBuilder.RenameIndex(
                name: "ix_project_members_project_id",
                table: "user_roles",
                newName: "ix_user_roles_project_id");

            migrationBuilder.AlterColumn<Guid>(
                name: "project_id",
                table: "user_roles",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<string>(
                name: "role_type",
                table: "user_roles",
                type: "character varying(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_roles",
                table: "user_roles",
                column: "user_role_id");

            migrationBuilder.AddForeignKey(
                name: "fk_user_roles_projects_project_id",
                table: "user_roles",
                column: "project_id",
                principalTable: "projects",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_roles_roles_role_id",
                table: "user_roles",
                column: "role_id",
                principalTable: "roles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_roles_users_user_id",
                table: "user_roles",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
