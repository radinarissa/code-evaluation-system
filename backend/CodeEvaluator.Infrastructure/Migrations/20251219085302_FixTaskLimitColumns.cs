using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeEvaluator.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixTaskLimitColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TimeLimitS",
                table: "Tasks",
                type: "integer",
                nullable: false,
                defaultValue: 3,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true,
                oldDefaultValue: 3m);

            migrationBuilder.AlterColumn<int>(
                name: "MemoryLimitKb",
                table: "Tasks",
                type: "integer",
                nullable: false,
                defaultValue: 262144,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                oldDefaultValue: 262144);

            migrationBuilder.AlterColumn<int>(
                name: "DiskLimitKb",
                table: "Tasks",
                type: "integer",
                nullable: false,
                defaultValue: 256,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                oldDefaultValue: 256);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TimeLimitS",
                table: "Tasks",
                type: "numeric",
                nullable: true,
                defaultValue: 3m,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 3);

            migrationBuilder.AlterColumn<int>(
                name: "MemoryLimitKb",
                table: "Tasks",
                type: "integer",
                nullable: true,
                defaultValue: 262144,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 262144);

            migrationBuilder.AlterColumn<int>(
                name: "DiskLimitKb",
                table: "Tasks",
                type: "integer",
                nullable: true,
                defaultValue: 256,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 256);
        }
    }
}
