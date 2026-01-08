using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeEvaluator.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMoodleAttemptNumberToSubmission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MoodleAttemptNumber",
                table: "Submissions",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MoodleAttemptNumber",
                table: "Submissions");
        }
    }
}
