using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeEvaluator.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSubmissionsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
