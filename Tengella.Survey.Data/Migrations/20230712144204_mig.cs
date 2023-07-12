using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tengella.Survey.Data.Migrations
{
    /// <inheritdoc />
    public partial class mig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Surveys",
                columns: new[] { "Id", "Name", "info" },
                values: new object[] { 1, "survey name", "this is a survey" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Surveys",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
