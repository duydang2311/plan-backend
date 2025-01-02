using Microsoft.EntityFrameworkCore.Migrations;
using WebApp.Domain.Constants;

#nullable disable

namespace WebApp.Host.Migrations
{
    /// <inheritdoc />
    public partial class _059_issueaudits_issueauditactionenum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:issue_audit_action", "create,update_description,update_title");

            migrationBuilder.AlterColumn<IssueAuditAction>(
                name: "action",
                table: "issue_audits",
                type: "issue_audit_action",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:Enum:issue_audit_action", "create,update_description,update_title");

            migrationBuilder.AlterColumn<string>(
                name: "action",
                table: "issue_audits",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(IssueAuditAction),
                oldType: "issue_audit_action");
        }
    }
}
