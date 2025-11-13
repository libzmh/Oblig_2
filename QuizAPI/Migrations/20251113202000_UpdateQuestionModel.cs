using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateQuestionModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CorrectAnswer",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "OptionA",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "OptionB",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "OptionC",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "OptionD",
                table: "Questions");

            migrationBuilder.AddColumn<string>(
                name: "CorrectAnswersJson",
                table: "Questions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OptionsJson",
                table: "Questions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Questions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CorrectAnswersJson",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "OptionsJson",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Questions");

            migrationBuilder.AddColumn<string>(
                name: "CorrectAnswer",
                table: "Questions",
                type: "TEXT",
                maxLength: 1,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OptionA",
                table: "Questions",
                type: "TEXT",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OptionB",
                table: "Questions",
                type: "TEXT",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OptionC",
                table: "Questions",
                type: "TEXT",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OptionD",
                table: "Questions",
                type: "TEXT",
                maxLength: 200,
                nullable: true);
        }
    }
}
