using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CodeEvaluator.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCoursesAndEnrollments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Courses_CourseId",
                table: "Tasks");

            migrationBuilder.DropTable(
                name: "CourseEnrollments");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "Tasks",
                newName: "MoodleCourseId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_CourseId",
                table: "Tasks",
                newName: "IX_Tasks_MoodleCourseId");

            migrationBuilder.AddColumn<int>(
                name: "AttemptNumber",
                table: "Submissions",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_MoodleSyncStatus",
                table: "Submissions",
                column: "MoodleSyncStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_Status",
                table: "Submissions",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_SubmissionTime",
                table: "Submissions",
                column: "SubmissionTime");

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_TaskId_UserId_AttemptNumber",
                table: "Submissions",
                columns: new[] { "TaskId", "UserId", "AttemptNumber" });

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_TaskId_UserId_SubmissionTime",
                table: "Submissions",
                columns: new[] { "TaskId", "UserId", "SubmissionTime" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Submissions_MoodleSyncStatus",
                table: "Submissions");

            migrationBuilder.DropIndex(
                name: "IX_Submissions_Status",
                table: "Submissions");

            migrationBuilder.DropIndex(
                name: "IX_Submissions_SubmissionTime",
                table: "Submissions");

            migrationBuilder.DropIndex(
                name: "IX_Submissions_TaskId_UserId_AttemptNumber",
                table: "Submissions");

            migrationBuilder.DropIndex(
                name: "IX_Submissions_TaskId_UserId_SubmissionTime",
                table: "Submissions");

            migrationBuilder.DropColumn(
                name: "AttemptNumber",
                table: "Submissions");

            migrationBuilder.RenameColumn(
                name: "MoodleCourseId",
                table: "Tasks",
                newName: "CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_MoodleCourseId",
                table: "Tasks",
                newName: "IX_Tasks_CourseId");

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AcademicYear = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    MoodleCourseId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Semester = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CourseEnrollments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CourseId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    EnrolledAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Role = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseEnrollments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseEnrollments_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseEnrollments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseEnrollments_CourseId_UserId",
                table: "CourseEnrollments",
                columns: new[] { "CourseId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourseEnrollments_UserId",
                table: "CourseEnrollments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_MoodleCourseId",
                table: "Courses",
                column: "MoodleCourseId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Courses_CourseId",
                table: "Tasks",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
