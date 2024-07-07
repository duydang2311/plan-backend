﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using WebApp.SharedKernel.Persistence;

#nullable disable

namespace WebApp.Host.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240707105001_010_policy")]
    partial class _010_policy
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("WebApp.SharedKernel.Models.JobRecord", b =>
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

            modelBuilder.Entity("WebApp.SharedKernel.Models.Policy", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Action")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasColumnName("action");

                    b.Property<string>("Domain")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasColumnName("domain");

                    b.Property<string>("Object")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasColumnName("object");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasColumnName("subject");

                    b.HasKey("Id")
                        .HasName("pk_policies");

                    b.HasIndex("Action")
                        .HasDatabaseName("ix_policies_action");

                    b.HasIndex("Domain")
                        .HasDatabaseName("ix_policies_domain");

                    b.HasIndex("Object")
                        .HasDatabaseName("ix_policies_object");

                    b.HasIndex("Subject")
                        .HasDatabaseName("ix_policies_subject");

                    b.ToTable("policies", (string)null);
                });

            modelBuilder.Entity("WebApp.SharedKernel.Models.Team", b =>
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
                        .HasColumnType("text")
                        .HasColumnName("identifier");

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

                    b.HasIndex("WorkspaceId")
                        .HasDatabaseName("ix_teams_workspace_id");

                    b.ToTable("teams", (string)null);
                });

            modelBuilder.Entity("WebApp.SharedKernel.Models.User", b =>
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
                        .IsUnique()
                        .HasDatabaseName("ix_users_email");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("WebApp.SharedKernel.Models.UserRefreshToken", b =>
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

            modelBuilder.Entity("WebApp.SharedKernel.Models.UserVerificationToken", b =>
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

            modelBuilder.Entity("WebApp.SharedKernel.Models.Workspace", b =>
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

            modelBuilder.Entity("team_members", b =>
                {
                    b.Property<Guid>("MembersId")
                        .HasColumnType("uuid")
                        .HasColumnName("members_id");

                    b.Property<Guid>("TeamsId")
                        .HasColumnType("uuid")
                        .HasColumnName("teams_id");

                    b.HasKey("MembersId", "TeamsId")
                        .HasName("pk_team_members");

                    b.HasIndex("TeamsId")
                        .HasDatabaseName("ix_team_members_teams_id");

                    b.ToTable("team_members", (string)null);
                });

            modelBuilder.Entity("WebApp.SharedKernel.Models.Team", b =>
                {
                    b.HasOne("WebApp.SharedKernel.Models.Workspace", "Workspace")
                        .WithMany()
                        .HasForeignKey("WorkspaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_teams_workspaces_workspace_id");

                    b.Navigation("Workspace");
                });

            modelBuilder.Entity("WebApp.SharedKernel.Models.UserRefreshToken", b =>
                {
                    b.HasOne("WebApp.SharedKernel.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_refresh_tokens_users_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("WebApp.SharedKernel.Models.UserVerificationToken", b =>
                {
                    b.HasOne("WebApp.SharedKernel.Models.User", "User")
                        .WithOne()
                        .HasForeignKey("WebApp.SharedKernel.Models.UserVerificationToken", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_verification_tokens_users_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("team_members", b =>
                {
                    b.HasOne("WebApp.SharedKernel.Models.User", null)
                        .WithMany()
                        .HasForeignKey("MembersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_team_members_users_members_id");

                    b.HasOne("WebApp.SharedKernel.Models.Team", null)
                        .WithMany()
                        .HasForeignKey("TeamsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_team_members_teams_teams_id");
                });
#pragma warning restore 612, 618
        }
    }
}
