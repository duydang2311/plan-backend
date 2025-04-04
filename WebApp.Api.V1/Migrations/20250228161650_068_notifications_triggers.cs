﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Host.Migrations
{
    /// <inheritdoc />
    public partial class _068_notifications_triggers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                create function delete_issue_created_notifications()
                returns trigger as
                $$
                begin
                    delete from "notifications" where "data"->>'issueId' = old.id::text;
                    return old;
                END;
                $$
                language plpgsql;

                create function delete_project_created_notifications()
                returns trigger as
                $$
                begin
                    delete from "notifications" where "data"->>'projectId' = old.id::text;
                    return old;
                END;
                $$
                language plpgsql;

                create function delete_issue_comment_created_notifications()
                returns trigger as
                $$
                begin
                    delete from "notifications" where ("data"->>'issueAuditId')::bigint = old.id;
                    return old;
                END;
                $$
                language plpgsql;

                create TRIGGER TR_issues_AD
                after delete on issues
                for each row
                execute function delete_issue_created_notifications();

                create TRIGGER TR_projects_AD
                after delete on projects
                for each row
                execute function delete_project_created_notifications();

                create TRIGGER TR_issue_audits_AD
                after delete on issue_audits
                for each row
                execute function delete_issue_comment_created_notifications();
                """
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                drop trigger TR_issues_AD on issues;
                drop function delete_issue_created_notifications();
                drop trigger TR_projects_AD on projects;
                drop function delete_project_created_notifications();
                drop trigger TR_issue_audits_AD on issue_audits;
                drop function delete_issue_comment_created_notifications();
                """
            );
        }
    }
}
