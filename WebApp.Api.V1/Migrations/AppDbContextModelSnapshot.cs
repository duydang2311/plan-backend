﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using WebApp.Infrastructure.Persistence;

#nullable disable

namespace WebApp.Host.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Casbin.Persist.Adapter.EFCore.Entities.EFCorePersistPolicy<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Type")
                        .HasColumnType("text")
                        .HasColumnName("ptype");

                    b.Property<string>("Value1")
                        .HasColumnType("text")
                        .HasColumnName("v0");

                    b.Property<string>("Value2")
                        .HasColumnType("text")
                        .HasColumnName("v1");

                    b.Property<string>("Value3")
                        .HasColumnType("text")
                        .HasColumnName("v2");

                    b.Property<string>("Value4")
                        .HasColumnType("text")
                        .HasColumnName("v3");

                    b.Property<string>("Value5")
                        .HasColumnType("text")
                        .HasColumnName("v4");

                    b.Property<string>("Value6")
                        .HasColumnType("text")
                        .HasColumnName("v5");

                    b.HasKey("Id")
                        .HasName("pk_policies");

                    b.HasIndex("Type")
                        .HasDatabaseName("ix_policies_ptype");

                    b.HasIndex("Value1")
                        .HasDatabaseName("ix_policies_v0");

                    b.HasIndex("Value2")
                        .HasDatabaseName("ix_policies_v1");

                    b.HasIndex("Value3")
                        .HasDatabaseName("ix_policies_v2");

                    b.HasIndex("Value4")
                        .HasDatabaseName("ix_policies_v3");

                    b.HasIndex("Value5")
                        .HasDatabaseName("ix_policies_v4");

                    b.HasIndex("Value6")
                        .HasDatabaseName("ix_policies_v5");

                    b.ToTable("policies", (string)null);
                });

            modelBuilder.Entity("WebApp.Domain.Entities.Issue", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uuid")
                        .HasColumnName("author_id");

                    b.Property<Instant>("CreatedTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_time")
                        .HasDefaultValueSql("now()");

                    b.Property<Instant?>("DeletedTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("deleted_time");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.Property<long>("OrderByStatus")
                        .HasColumnType("bigint")
                        .HasColumnName("order_by_status");

                    b.Property<long>("OrderNumber")
                        .HasColumnType("bigint")
                        .HasColumnName("order_number");

                    b.Property<long?>("StatusId")
                        .HasColumnType("bigint")
                        .HasColumnName("status_id");

                    b.Property<Guid>("TeamId")
                        .HasColumnType("uuid")
                        .HasColumnName("team_id");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasColumnName("title");

                    b.Property<Instant>("UpdatedTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_time")
                        .HasDefaultValueSql("now()");

                    b.HasKey("Id")
                        .HasName("pk_issues");

                    b.HasIndex("AuthorId")
                        .HasDatabaseName("ix_issues_author_id");

                    b.HasIndex("StatusId")
                        .HasDatabaseName("ix_issues_status_id");

                    b.HasIndex("TeamId", "OrderNumber")
                        .IsUnique()
                        .HasDatabaseName("ix_issues_team_id_order_number");

                    b.ToTable("issues", (string)null);
                });

            modelBuilder.Entity("WebApp.Domain.Entities.IssueComment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uuid")
                        .HasColumnName("author_id");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("content");

                    b.Property<Instant>("CreatedTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_time")
                        .HasDefaultValueSql("now()");

                    b.Property<Guid>("IssueId")
                        .HasColumnType("uuid")
                        .HasColumnName("issue_id");

                    b.Property<Instant>("UpdatedTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_time")
                        .HasDefaultValueSql("now()");

                    b.HasKey("Id")
                        .HasName("pk_issue_comments");

                    b.HasIndex("AuthorId")
                        .HasDatabaseName("ix_issue_comments_author_id");

                    b.HasIndex("IssueId")
                        .HasDatabaseName("ix_issue_comments_issue_id");

                    b.ToTable("issue_comments", (string)null);
                });

            modelBuilder.Entity("WebApp.Domain.Entities.IssueField", b =>
                {
                    b.Property<Guid>("IssueId")
                        .HasColumnType("uuid")
                        .HasColumnName("issue_id");

                    b.Property<string>("Name")
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasColumnName("name");

                    b.Property<byte>("discriminator")
                        .HasColumnType("smallint")
                        .HasColumnName("discriminator");

                    b.HasKey("IssueId", "Name")
                        .HasName("pk_issue_fields");

                    b.ToTable("issue_fields", (string)null);

                    b.HasDiscriminator<byte>("discriminator");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("WebApp.Domain.Entities.JobRecord", b =>
                {
                    b.Property<Guid>("TrackingID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("tracking_id");

                    b.Property<string>("CommandJson")
                        .IsRequired()
                        .HasColumnType("jsonb")
                        .HasColumnName("command_json");

                    b.Property<DateTime>("ExecuteAfter")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("execute_after");

                    b.Property<DateTime>("ExpireOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("expire_on");

                    b.Property<bool>("IsComplete")
                        .HasColumnType("boolean")
                        .HasColumnName("is_complete");

                    b.Property<string>("QueueID")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasColumnName("queue_id");

                    b.HasKey("TrackingID")
                        .HasName("pk_job_records");

                    b.ToTable("job_records", (string)null);
                });

            modelBuilder.Entity("WebApp.Domain.Entities.SharedCounter", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<long>("Count")
                        .HasColumnType("bigint")
                        .HasColumnName("count");

                    b.HasKey("Id")
                        .HasName("pk_shared_counters");

                    b.ToTable("shared_counters", (string)null);
                });

            modelBuilder.Entity("WebApp.Domain.Entities.Status", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("character varying(16)")
                        .HasColumnName("color");

                    b.Property<string>("Description")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("description");

                    b.Property<int>("Order")
                        .HasColumnType("integer")
                        .HasColumnName("order");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasColumnName("value");

                    b.HasKey("Id")
                        .HasName("pk_statuses");

                    b.ToTable("statuses", (string)null);
                });

            modelBuilder.Entity("WebApp.Domain.Entities.Team", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Instant>("CreatedTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_time")
                        .HasDefaultValueSql("now()");

                    b.Property<string>("Identifier")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("character varying(5)")
                        .HasColumnName("identifier")
                        .UseCollation("case_insensitive");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<Instant>("UpdatedTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_time")
                        .HasDefaultValueSql("now()");

                    b.Property<Guid>("WorkspaceId")
                        .HasColumnType("uuid")
                        .HasColumnName("workspace_id");

                    b.HasKey("Id")
                        .HasName("pk_teams");

                    b.HasIndex("WorkspaceId", "Identifier")
                        .IsUnique()
                        .HasDatabaseName("ix_teams_workspace_id_identifier");

                    b.ToTable("teams", (string)null);
                });

            modelBuilder.Entity("WebApp.Domain.Entities.TeamInvitation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Instant>("CreatedTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_time")
                        .HasDefaultValueSql("now()");

                    b.Property<Guid>("MemberId")
                        .HasColumnType("uuid")
                        .HasColumnName("member_id");

                    b.Property<Guid>("TeamId")
                        .HasColumnType("uuid")
                        .HasColumnName("team_id");

                    b.HasKey("Id")
                        .HasName("pk_team_invitations");

                    b.HasIndex("MemberId")
                        .HasDatabaseName("ix_team_invitations_member_id");

                    b.HasIndex("TeamId", "MemberId")
                        .IsUnique()
                        .HasDatabaseName("ix_team_invitations_team_id_member_id");

                    b.ToTable("team_invitations", (string)null);
                });

            modelBuilder.Entity("WebApp.Domain.Entities.TeamMember", b =>
                {
                    b.Property<Guid>("TeamId")
                        .HasColumnType("uuid")
                        .HasColumnName("team_id");

                    b.Property<Guid>("MemberId")
                        .HasColumnType("uuid")
                        .HasColumnName("member_id");

                    b.Property<Instant>("CreatedTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_time")
                        .HasDefaultValueSql("now()");

                    b.Property<int>("RoleId")
                        .HasColumnType("integer")
                        .HasColumnName("role_id");

                    b.Property<Instant>("UpdatedTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_time")
                        .HasDefaultValueSql("now()");

                    b.HasKey("TeamId", "MemberId")
                        .HasName("pk_team_members");

                    b.HasIndex("MemberId")
                        .HasDatabaseName("ix_team_members_member_id");

                    b.HasIndex("RoleId")
                        .HasDatabaseName("ix_team_members_role_id");

                    b.ToTable("team_members", (string)null);
                });

            modelBuilder.Entity("WebApp.Domain.Entities.TeamRole", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_team_roles");

                    b.ToTable("team_roles", (string)null);
                });

            modelBuilder.Entity("WebApp.Domain.Entities.TeamRolePermission", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("role_id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.None);

                    b.Property<string>("Permission")
                        .HasColumnType("text")
                        .HasColumnName("permission");

                    b.HasKey("RoleId", "Permission")
                        .HasName("pk_team_role_permissions");

                    b.HasIndex("RoleId")
                        .HasDatabaseName("ix_team_role_permissions_role_id");

                    b.ToTable("team_role_permissions", (string)null);
                });

            modelBuilder.Entity("WebApp.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Instant>("CreatedTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_time")
                        .HasDefaultValueSql("now()");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(254)
                        .HasColumnType("character varying(254)")
                        .HasColumnName("email");

                    b.Property<bool>("IsVerified")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("is_verified");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("bytea")
                        .HasColumnName("password_hash");

                    b.Property<byte[]>("Salt")
                        .IsRequired()
                        .HasColumnType("bytea")
                        .HasColumnName("salt");

                    b.Property<Instant>("UpdatedTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_time")
                        .HasDefaultValueSql("now()");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.HasIndex("Email")
                        .HasDatabaseName("ix_users_email");

                    NpgsqlIndexBuilderExtensions.HasMethod(b.HasIndex("Email"), "gin");
                    NpgsqlIndexBuilderExtensions.HasOperators(b.HasIndex("Email"), new[] { "gin_trgm_ops" });

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("WebApp.Domain.Entities.UserRefreshToken", b =>
                {
                    b.Property<Guid>("Token")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("token");

                    b.Property<Instant>("CreatedTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_time")
                        .HasDefaultValueSql("now()");

                    b.Property<Instant>("UpdatedTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_time")
                        .HasDefaultValueSql("now()");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Token")
                        .HasName("pk_user_refresh_tokens");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_user_refresh_tokens_user_id");

                    b.ToTable("user_refresh_tokens", (string)null);
                });

            modelBuilder.Entity("WebApp.Domain.Entities.UserVerificationToken", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.Property<Guid>("Token")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("token");

                    b.HasKey("UserId", "Token")
                        .HasName("pk_user_verification_tokens");

                    b.HasIndex("UserId")
                        .IsUnique()
                        .HasDatabaseName("ix_user_verification_tokens_user_id");

                    b.ToTable("user_verification_tokens", (string)null);
                });

            modelBuilder.Entity("WebApp.Domain.Entities.Workspace", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Instant>("CreatedTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_time")
                        .HasDefaultValueSql("now()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasColumnName("name");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasColumnName("path")
                        .UseCollation("case_insensitive");

                    b.Property<Instant>("UpdatedTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_time")
                        .HasDefaultValueSql("now()");

                    b.HasKey("Id")
                        .HasName("pk_workspaces");

                    b.HasIndex("Path")
                        .IsUnique()
                        .HasDatabaseName("ix_workspaces_path");

                    b.ToTable("workspaces", (string)null);
                });

            modelBuilder.Entity("WebApp.Domain.Entities.WorkspaceFieldDefinition", b =>
                {
                    b.Property<Guid>("WorkspaceId")
                        .HasColumnType("uuid")
                        .HasColumnName("workspace_id");

                    b.Property<string>("Name")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasColumnName("name");

                    b.Property<string>("Description")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("description");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("character varying(16)")
                        .HasColumnName("type");

                    b.HasKey("WorkspaceId", "Name")
                        .HasName("pk_workspace_field_definitions");

                    b.ToTable("workspace_field_definitions", (string)null);
                });

            modelBuilder.Entity("workspace_statuses", b =>
                {
                    b.Property<long>("StatusId")
                        .HasColumnType("bigint")
                        .HasColumnName("status_id");

                    b.Property<Guid>("WorkspaceId")
                        .HasColumnType("uuid")
                        .HasColumnName("workspace_id");

                    b.HasKey("StatusId")
                        .HasName("pk_workspace_statuses");

                    b.HasIndex("WorkspaceId")
                        .HasDatabaseName("ix_workspace_statuses_workspace_id");

                    b.ToTable("workspace_statuses", (string)null);
                });

            modelBuilder.Entity("WebApp.Domain.Entities.IssueFieldBoolean", b =>
                {
                    b.HasBaseType("WebApp.Domain.Entities.IssueField");

                    b.Property<bool>("Value")
                        .HasColumnType("boolean")
                        .HasColumnName("value");

                    b.ToTable("issue_fields", (string)null);

                    b.HasDiscriminator().HasValue((byte)2);
                });

            modelBuilder.Entity("WebApp.Domain.Entities.IssueFieldNumber", b =>
                {
                    b.HasBaseType("WebApp.Domain.Entities.IssueField");

                    b.Property<int>("Value")
                        .HasColumnType("integer")
                        .HasColumnName("value");

                    b.ToTable("issue_fields", null, t =>
                        {
                            t.Property("Value")
                                .HasColumnName("issue_field_number_value");
                        });

                    b.HasDiscriminator().HasValue((byte)1);
                });

            modelBuilder.Entity("WebApp.Domain.Entities.IssueFieldText", b =>
                {
                    b.HasBaseType("WebApp.Domain.Entities.IssueField");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("value");

                    b.ToTable("issue_fields", null, t =>
                        {
                            t.Property("Value")
                                .HasColumnName("issue_field_text_value");
                        });

                    b.HasDiscriminator().HasValue((byte)0);
                });

            modelBuilder.Entity("WebApp.Domain.Entities.Issue", b =>
                {
                    b.HasOne("WebApp.Domain.Entities.User", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_issues_users_author_id");

                    b.HasOne("WebApp.Domain.Entities.Status", "Status")
                        .WithMany()
                        .HasForeignKey("StatusId")
                        .HasConstraintName("fk_issues_statuses_status_id");

                    b.HasOne("WebApp.Domain.Entities.Team", "Team")
                        .WithMany("Issues")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_issues_teams_team_id");

                    b.Navigation("Author");

                    b.Navigation("Status");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("WebApp.Domain.Entities.IssueComment", b =>
                {
                    b.HasOne("WebApp.Domain.Entities.User", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_issue_comments_users_author_id");

                    b.HasOne("WebApp.Domain.Entities.Issue", "Issue")
                        .WithMany("Comments")
                        .HasForeignKey("IssueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_issue_comments_issues_issue_id");

                    b.Navigation("Author");

                    b.Navigation("Issue");
                });

            modelBuilder.Entity("WebApp.Domain.Entities.IssueField", b =>
                {
                    b.HasOne("WebApp.Domain.Entities.Issue", "Issue")
                        .WithMany("Fields")
                        .HasForeignKey("IssueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_issue_fields_issues_issue_id");

                    b.Navigation("Issue");
                });

            modelBuilder.Entity("WebApp.Domain.Entities.Team", b =>
                {
                    b.HasOne("WebApp.Domain.Entities.Workspace", "Workspace")
                        .WithMany()
                        .HasForeignKey("WorkspaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_teams_workspaces_workspace_id");

                    b.Navigation("Workspace");
                });

            modelBuilder.Entity("WebApp.Domain.Entities.TeamInvitation", b =>
                {
                    b.HasOne("WebApp.Domain.Entities.User", "Member")
                        .WithMany()
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_team_invitations_users_member_id");

                    b.HasOne("WebApp.Domain.Entities.Team", "Team")
                        .WithMany()
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_team_invitations_teams_team_id");

                    b.HasOne("WebApp.Domain.Entities.TeamMember", null)
                        .WithOne("PendingInvitation")
                        .HasForeignKey("WebApp.Domain.Entities.TeamInvitation", "TeamId", "MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_team_invitations_team_members_team_id_member_id");

                    b.Navigation("Member");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("WebApp.Domain.Entities.TeamMember", b =>
                {
                    b.HasOne("WebApp.Domain.Entities.User", "Member")
                        .WithMany()
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_team_members_users_member_id");

                    b.HasOne("WebApp.Domain.Entities.TeamRole", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_team_members_team_roles_role_id");

                    b.HasOne("WebApp.Domain.Entities.Team", "Team")
                        .WithMany()
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_team_members_teams_team_id");

                    b.Navigation("Member");

                    b.Navigation("Role");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("WebApp.Domain.Entities.TeamRolePermission", b =>
                {
                    b.HasOne("WebApp.Domain.Entities.TeamRole", null)
                        .WithMany("Permissions")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_team_role_permissions_team_roles_role_id");
                });

            modelBuilder.Entity("WebApp.Domain.Entities.UserRefreshToken", b =>
                {
                    b.HasOne("WebApp.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_refresh_tokens_users_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("WebApp.Domain.Entities.UserVerificationToken", b =>
                {
                    b.HasOne("WebApp.Domain.Entities.User", "User")
                        .WithOne()
                        .HasForeignKey("WebApp.Domain.Entities.UserVerificationToken", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_verification_tokens_users_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("WebApp.Domain.Entities.WorkspaceFieldDefinition", b =>
                {
                    b.HasOne("WebApp.Domain.Entities.Workspace", "Workspace")
                        .WithMany("FieldDefinitions")
                        .HasForeignKey("WorkspaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_workspace_field_definitions_workspaces_workspace_id");

                    b.Navigation("Workspace");
                });

            modelBuilder.Entity("workspace_statuses", b =>
                {
                    b.HasOne("WebApp.Domain.Entities.Status", "Status")
                        .WithMany()
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_workspace_statuses_statuses_status_id");

                    b.HasOne("WebApp.Domain.Entities.Workspace", "Workspace")
                        .WithMany()
                        .HasForeignKey("WorkspaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_workspace_statuses_workspaces_workspace_id");

                    b.Navigation("Status");

                    b.Navigation("Workspace");
                });

            modelBuilder.Entity("WebApp.Domain.Entities.Issue", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Fields");
                });

            modelBuilder.Entity("WebApp.Domain.Entities.Team", b =>
                {
                    b.Navigation("Issues");
                });

            modelBuilder.Entity("WebApp.Domain.Entities.TeamMember", b =>
                {
                    b.Navigation("PendingInvitation");
                });

            modelBuilder.Entity("WebApp.Domain.Entities.TeamRole", b =>
                {
                    b.Navigation("Permissions");
                });

            modelBuilder.Entity("WebApp.Domain.Entities.Workspace", b =>
                {
                    b.Navigation("FieldDefinitions");
                });
#pragma warning restore 612, 618
        }
    }
}
