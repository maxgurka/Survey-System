using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tengella.Survey.Data.Migrations
{
    /// <inheritdoc />
    public partial class RecipientLists : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RecipientLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipientLists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RecipientRecipientList",
                columns: table => new
                {
                    RecipientListsId = table.Column<int>(type: "int", nullable: false),
                    RecipientsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipientRecipientList", x => new { x.RecipientListsId, x.RecipientsId });
                    table.ForeignKey(
                        name: "FK_RecipientRecipientList_RecipientLists_RecipientListsId",
                        column: x => x.RecipientListsId,
                        principalTable: "RecipientLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecipientRecipientList_Recipients_RecipientsId",
                        column: x => x.RecipientsId,
                        principalTable: "Recipients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecipientRecipientList_RecipientsId",
                table: "RecipientRecipientList",
                column: "RecipientsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecipientRecipientList");

            migrationBuilder.DropTable(
                name: "RecipientLists");
        }
    }
}
