﻿// <auto-generated />
using System;
using Course.Management.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Course.Management.Api.Migrations
{
    [DbContext(typeof(CourseContext))]
    [Migration("20230531182240_ThirdMigration")]
    partial class ThirdMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Course.Management.Api.Data.Course", b =>
                {
                    b.Property<string>("CourseId")
                        .HasColumnType("text");

                    b.Property<string>("CourseDescription")
                        .HasColumnType("text");

                    b.Property<string>("CourseName")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("CourseId");

                    b.ToTable("Courses");
                });
#pragma warning restore 612, 618
        }
    }
}
