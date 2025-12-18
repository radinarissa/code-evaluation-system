using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeEvaluator.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatesBecaseofJudge0 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiskLimitMb",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "MemoryLimitMb",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "TimeLimitMs",
                table: "Tasks");

            migrationBuilder.AddColumn<int>(
                name: "DiskLimitKb",
                table: "Tasks",
                type: "integer",
                nullable: true,
                defaultValue: 256);

            migrationBuilder.AddColumn<int>(
                name: "MemoryLimitKb",
                table: "Tasks",
                type: "integer",
                nullable: true,
                defaultValue: 262144);

            migrationBuilder.AddColumn<int>(
                name: "StackLimitKb",
                table: "Tasks",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TimeLimitS",
                table: "Tasks",
                type: "numeric",
                nullable: true,
                defaultValue: 3m);

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
                name: "DiskLimitKb",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "MemoryLimitKb",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "StackLimitKb",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "TimeLimitS",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "AttemptNumber",
                table: "Submissions");

            migrationBuilder.AddColumn<int>(
                name: "DiskLimitMb",
                table: "Tasks",
                type: "integer",
                nullable: false,
                defaultValue: 256);

            migrationBuilder.AddColumn<int>(
                name: "MemoryLimitMb",
                table: "Tasks",
                type: "integer",
                nullable: false,
                defaultValue: 256);

            migrationBuilder.AddColumn<int>(
                name: "TimeLimitMs",
                table: "Tasks",
                type: "integer",
                nullable: false,
                defaultValue: 5000);
        }
    }
}
