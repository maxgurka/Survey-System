using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tengella.Survey.Data.Migrations
{
    /// <inheritdoc />
    public partial class RecipientRespondentConnection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RecipientId",
                table: "Respondents",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Respondents_RecipientId",
                table: "Respondents",
                column: "RecipientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Respondents_Recipients_RecipientId",
                table: "Respondents",
                column: "RecipientId",
                principalTable: "Recipients",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Respondents_Recipients_RecipientId",
                table: "Respondents");

            migrationBuilder.DropIndex(
                name: "IX_Respondents_RecipientId",
                table: "Respondents");

            migrationBuilder.DropColumn(
                name: "RecipientId",
                table: "Respondents");
        }
    }
}
