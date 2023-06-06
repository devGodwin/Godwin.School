using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GradeAndAssessment.Api.Migrations
{
    public partial class FourthMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StudentAssessments",
                columns: table => new
                {
                    AssessmentId = table.Column<string>(type: "text", nullable: false),
                    TeacherName = table.Column<string>(type: "text", nullable: true),
                    IndexNumber = table.Column<string>(type: "text", nullable: true),
                    CourseId = table.Column<string>(type: "text", nullable: true),
                    Assignment = table.Column<decimal>(type: "numeric", nullable: false),
                    Project = table.Column<decimal>(type: "numeric", nullable: false),
                    MidsemExam = table.Column<decimal>(type: "numeric", nullable: false),
                    EndOfSemExam = table.Column<decimal>(type: "numeric", nullable: false),
                    Score = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "bytea", nullable: true),
                    PasswordSalt = table.Column<byte[]>(type: "bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentAssessments", x => x.AssessmentId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentAssessments");
        }
    }
}
