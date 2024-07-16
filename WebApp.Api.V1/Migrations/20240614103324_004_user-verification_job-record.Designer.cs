﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using WebApp.Infrastructure.Persistence;

#nullable disable

namespace WebApp.Host.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240614103324_004_user-verification_job-record")]
    partial class _004_userverification_jobrecord
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("WebApp.Common.Models.JobRecord", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

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
                        .HasColumnType("text")
                        .HasColumnName("queue_id");

                    b.HasKey("Id")
                        .HasName("pk_job_records");

                    b.ToTable("job_records", (string)null);
                });

            modelBuilder.Entity("WebApp.Common.Models.User", b =>
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

            modelBuilder.Entity("WebApp.Common.Models.UserVerificationToken", b =>
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

            modelBuilder.Entity("WebApp.Common.Models.UserVerificationToken", b =>
                {
                    b.HasOne("WebApp.Common.Models.User", "User")
                        .WithOne()
                        .HasForeignKey("WebApp.Common.Models.UserVerificationToken", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_verification_tokens_users_user_id");

                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}
