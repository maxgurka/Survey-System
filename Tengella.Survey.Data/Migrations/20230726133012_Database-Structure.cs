using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tengella.Survey.Data.Migrations
{
    /// <inheritdoc />
    public partial class DatabaseStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SurveyId",
                table: "Respondents",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Respondents_SurveyId",
                table: "Respondents",
                column: "SurveyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Respondents_Surveys_SurveyId",
                table: "Respondents",
                column: "SurveyId",
                principalTable: "Surveys",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Respondents_Surveys_SurveyId",
                table: "Respondents");

            migrationBuilder.DropIndex(
                name: "IX_Respondents_SurveyId",
                table: "Respondents");

            migrationBuilder.DropColumn(
                name: "SurveyId",
                table: "Respondents");
        }
    }
}
