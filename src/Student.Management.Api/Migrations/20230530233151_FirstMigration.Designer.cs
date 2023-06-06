﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Student.Management.Api.Data;

#nullable disable

namespace Student.Management.Api.Migrations
{
    [DbContext(typeof(StudentContext))]
    [Migration("20230530233151_FirstMigration")]
    partial class FirstMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Student.Management.Api.Data.Student", b =>
                {
                    b.Property<string>("IndexNumber")
                        .HasColumnType("text");

                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<string>("ContactNumber")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("DateOfBirth")
                        .HasColumnType("text");

                    b.Property<int>("DbCounter")
                        .HasColumnType("integer");

                    b.Property<string>("EmailAddress")
                        .HasColumnType("text");

                    b.Property<string>("EmergencyContactName")
                        .HasColumnType("text");

                    b.Property<string>("EmergencyContactNumber")
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<byte[]>("PasswordHash")
                        .HasColumnType("bytea");

                    b.Property<byte[]>("PasswordSalt")
                        .HasColumnType("bytea");

                    b.HasKey("IndexNumber");

                    b.ToTable("Students");
                });
#pragma warning restore 612, 618
        }
    }
}
