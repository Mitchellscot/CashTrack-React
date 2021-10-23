using Microsoft.EntityFrameworkCore.Migrations;

namespace CashTrack.Migrations
{
    public partial class fixingtagtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Expenses_ExpenseId",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_ExpenseId",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "ExpenseId",
                table: "Tags");

            migrationBuilder.CreateTable(
                name: "ExpenseTag",
                columns: table => new
                {
                    ExpensesId = table.Column<int>(type: "integer", nullable: false),
                    TagsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseTag", x => new { x.ExpensesId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_ExpenseTag_Expenses_ExpensesId",
                        column: x => x.ExpensesId,
                        principalTable: "Expenses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExpenseTag_Tags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$hrAQGDWL7LSYZXerYtR/FOdEel3wfTkuoj0/gVXntRm9ZNMAZTLrO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "$2a$11$LRftgoDOq0lpiJM5nj/w.e7jj1VDPbBgfvjADvh7dSrO1tVKcuKrC");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseTag_TagsId",
                table: "ExpenseTag",
                column: "TagsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExpenseTag");

            migrationBuilder.AddColumn<int>(
                name: "ExpenseId",
                table: "Tags",
                type: "integer",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$jc43S4shjgFNraSqnfA26e17IGAid3XMHvE1As7cBDEnwSLWPf8xq");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "$2a$11$hW1VPw9S18iDA0vNiiDWa.7G813JTVZWnDlYNizZ5hRJh/RT0d/aK");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_ExpenseId",
                table: "Tags",
                column: "ExpenseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Expenses_ExpenseId",
                table: "Tags",
                column: "ExpenseId",
                principalTable: "Expenses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
