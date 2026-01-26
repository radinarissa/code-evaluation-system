using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CodeEvaluator.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialReal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MoodleId = table.Column<int>(type: "integer", nullable: false),
                    Username = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    FirstName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    LastName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Role = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MoodleCourseId = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    MaxPoints = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false, defaultValue: 10.00m),
                    TimeLimitS = table.Column<int>(type: "integer", nullable: false, defaultValue: 3),
                    MemoryLimitKb = table.Column<int>(type: "integer", nullable: false, defaultValue: 262144),
                    DiskLimitKb = table.Column<int>(type: "integer", nullable: false, defaultValue: 256),
                    StackLimitKb = table.Column<int>(type: "integer", nullable: true),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    DueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MoodleAssignmentId = table.Column<int>(type: "integer", nullable: true),
                    MoodleAssignmentName = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AdditionalFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TaskId = table.Column<int>(type: "integer", nullable: false),
                    Filename = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    FileContent = table.Column<byte[]>(type: "bytea", nullable: false),
                    FileSize = table.Column<int>(type: "integer", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionalFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdditionalFiles_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReferenceSolutions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TaskId = table.Column<int>(type: "integer", nullable: false),
                    SourceCode = table.Column<string>(type: "text", nullable: false),
                    UploadedBy = table.Column<int>(type: "integer", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    IsValid = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    ValidationMessage = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReferenceSolutions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReferenceSolutions_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReferenceSolutions_Users_UploadedBy",
                        column: x => x.UploadedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Submissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TaskId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    AttemptNumber = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    SubmissionTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "Pending"),
                    FinalGrade = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    Feedback = table.Column<string>(type: "text", nullable: true),
                    CompilationOutput = table.Column<string>(type: "text", nullable: true),
                    EvaluationStartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EvaluationCompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MoodleSubmissionId = table.Column<int>(type: "integer", nullable: true),
                    MoodleAttemptNumber = table.Column<int>(type: "integer", nullable: true),
                    MoodleSyncStatus = table.Column<string>(type: "text", nullable: true),
                    MoodleSyncOutput = table.Column<string>(type: "text", nullable: true),
                    MoodleSyncCreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Submissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Submissions_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Submissions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TestCases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TaskId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Input = table.Column<string>(type: "text", nullable: false),
                    ExpectedOutput = table.Column<string>(type: "text", nullable: false),
                    IsPublic = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Points = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false, defaultValue: 1.00m),
                    ExecutionOrder = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestCases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestCases_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TestResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TestCaseId = table.Column<int>(type: "integer", nullable: false),
                    SubmissionId = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ExecutionTime = table.Column<float>(type: "real", nullable: false),
                    MemoryUsage = table.Column<float>(type: "real", nullable: true),
                    DiskUsedMb = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    Output = table.Column<string>(type: "text", nullable: true),
                    ErrorMessage = table.Column<string>(type: "text", nullable: true),
                    Judge0Token = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestResults_Submissions_SubmissionId",
                        column: x => x.SubmissionId,
                        principalTable: "Submissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TestResults_TestCases_TestCaseId",
                        column: x => x.TestCaseId,
                        principalTable: "TestCases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalFiles_TaskId",
                table: "AdditionalFiles",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_ReferenceSolutions_TaskId",
                table: "ReferenceSolutions",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_ReferenceSolutions_UploadedBy",
                table: "ReferenceSolutions",
                column: "UploadedBy");

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
                name: "IX_Submissions_TaskId",
                table: "Submissions",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_TaskId_UserId_AttemptNumber",
                table: "Submissions",
                columns: new[] { "TaskId", "UserId", "AttemptNumber" });

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_TaskId_UserId_SubmissionTime",
                table: "Submissions",
                columns: new[] { "TaskId", "UserId", "SubmissionTime" });

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_UserId",
                table: "Submissions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_CreatedBy",
                table: "Tasks",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_MoodleAssignmentId",
                table: "Tasks",
                column: "MoodleAssignmentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_MoodleCourseId",
                table: "Tasks",
                column: "MoodleCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_TestCases_TaskId",
                table: "TestCases",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_TestResults_SubmissionId",
                table: "TestResults",
                column: "SubmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_TestResults_TestCaseId",
                table: "TestResults",
                column: "TestCaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_MoodleId",
                table: "Users",
                column: "MoodleId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdditionalFiles");

            migrationBuilder.DropTable(
                name: "ReferenceSolutions");

            migrationBuilder.DropTable(
                name: "TestResults");

            migrationBuilder.DropTable(
                name: "Submissions");

            migrationBuilder.DropTable(
                name: "TestCases");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
