using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace CashTrack.Migrations
{
    public partial class MerchantsAndTagsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Merchant",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "Tags",
                table: "Expenses");

            migrationBuilder.AddColumn<int>(
                name: "MerchantId",
                table: "Expenses",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Merchants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    SuggestOnLookup = table.Column<bool>(type: "boolean", nullable: false),
                    City = table.Column<string>(type: "text", nullable: true),
                    State = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Merchants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TagName = table.Column<string>(type: "text", nullable: true),
                    ExpenseId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tags_Expenses_ExpenseId",
                        column: x => x.ExpenseId,
                        principalTable: "Expenses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 1,
                column: "Catagory",
                value: 8);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 2,
                column: "Catagory",
                value: 15);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 3,
                column: "Catagory",
                value: 6);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 4,
                column: "Catagory",
                value: 2);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 5,
                column: "Catagory",
                value: 13);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 6,
                column: "Catagory",
                value: 13);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 7,
                column: "Catagory",
                value: 13);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 8,
                column: "Catagory",
                value: 13);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 9,
                column: "Catagory",
                value: 4);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 10,
                column: "Catagory",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 11,
                column: "Catagory",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 12,
                column: "Catagory",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 13,
                column: "Catagory",
                value: 6);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 14,
                column: "Catagory",
                value: 5);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 15,
                column: "Catagory",
                value: 9);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 16,
                column: "Catagory",
                value: 3);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 17,
                column: "Catagory",
                value: 13);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 18,
                column: "Catagory",
                value: 5);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 19,
                column: "Catagory",
                value: 5);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 20,
                column: "Catagory",
                value: 5);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 21,
                column: "Catagory",
                value: 5);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 22,
                column: "Catagory",
                value: 6);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 23,
                column: "Catagory",
                value: 14);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 24,
                column: "Catagory",
                value: 7);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 25,
                column: "Catagory",
                value: 11);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 26,
                column: "Catagory",
                value: 7);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 27,
                column: "Catagory",
                value: 7);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 28,
                column: "Catagory",
                value: 6);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 29,
                column: "Catagory",
                value: 13);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 30,
                column: "Catagory",
                value: 4);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 31,
                column: "Catagory",
                value: 3);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 32,
                column: "Catagory",
                value: 11);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 33,
                column: "Catagory",
                value: 6);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 34,
                column: "Catagory",
                value: 7);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 35,
                column: "Catagory",
                value: 9);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 36,
                column: "Catagory",
                value: 10);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 37,
                column: "Catagory",
                value: 14);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 38,
                column: "Catagory",
                value: 9);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 39,
                column: "Catagory",
                value: 7);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 40,
                column: "Catagory",
                value: 7);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 41,
                column: "Catagory",
                value: 8);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 42,
                column: "Catagory",
                value: 6);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 43,
                column: "Catagory",
                value: 10);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 44,
                column: "Catagory",
                value: 2);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 45,
                column: "Catagory",
                value: 11);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 46,
                column: "Catagory",
                value: 2);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 47,
                column: "Catagory",
                value: 14);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 48,
                column: "Catagory",
                value: 7);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 49,
                column: "Catagory",
                value: 11);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 50,
                column: "Catagory",
                value: 6);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 51,
                column: "Catagory",
                value: 6);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 52,
                column: "Catagory",
                value: 14);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 53,
                column: "Catagory",
                value: 10);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 54,
                column: "Catagory",
                value: 12);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 55,
                column: "Catagory",
                value: 10);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 56,
                column: "Catagory",
                value: 6);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 57,
                column: "Catagory",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 58,
                column: "Catagory",
                value: 11);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 59,
                column: "Catagory",
                value: 6);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 60,
                column: "Catagory",
                value: 11);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 61,
                column: "Catagory",
                value: 6);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 62,
                column: "Catagory",
                value: 7);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 63,
                columns: new[] { "Catagory", "Name" },
                values: new object[] { 0, "Software Development" });

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 64,
                columns: new[] { "Catagory", "InUse", "Name" },
                values: new object[] { 6, true, "Swimming" });

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 65,
                columns: new[] { "Catagory", "InUse", "Name" },
                values: new object[] { 11, false, "Taxes" });

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 66,
                columns: new[] { "Catagory", "Name" },
                values: new object[] { 7, "Toiletries" });

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 67,
                columns: new[] { "Catagory", "Name" },
                values: new object[] { 9, "Toys" });

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 68,
                columns: new[] { "Catagory", "Name" },
                values: new object[] { 15, "Trash" });

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 69,
                columns: new[] { "Catagory", "Name" },
                values: new object[] { 15, "Travel Misc" });

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 70,
                columns: new[] { "Catagory", "Name" },
                values: new object[] { 14, "Water" });

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 71,
                column: "Name",
                value: "Weight Training");

            migrationBuilder.InsertData(
                table: "ExpenseCatagories",
                columns: new[] { "Id", "Catagory", "InUse", "Name" },
                values: new object[] { 72, 7, true, "Yard" });

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
                name: "IX_Expenses_MerchantId",
                table: "Expenses",
                column: "MerchantId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_ExpenseId",
                table: "Tags",
                column: "ExpenseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Merchants_MerchantId",
                table: "Expenses",
                column: "MerchantId",
                principalTable: "Merchants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Merchants_MerchantId",
                table: "Expenses");

            migrationBuilder.DropTable(
                name: "Merchants");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_MerchantId",
                table: "Expenses");

            migrationBuilder.DeleteData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 72);

            migrationBuilder.DropColumn(
                name: "MerchantId",
                table: "Expenses");

            migrationBuilder.AddColumn<string>(
                name: "Merchant",
                table: "Expenses",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<List<string>>(
                name: "Tags",
                table: "Expenses",
                type: "text[]",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 1,
                column: "Catagory",
                value: 7);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 2,
                column: "Catagory",
                value: 14);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 3,
                column: "Catagory",
                value: 5);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 4,
                column: "Catagory",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 5,
                column: "Catagory",
                value: 12);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 6,
                column: "Catagory",
                value: 12);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 7,
                column: "Catagory",
                value: 12);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 8,
                column: "Catagory",
                value: 12);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 9,
                column: "Catagory",
                value: 3);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 10,
                column: "Catagory",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 11,
                column: "Catagory",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 12,
                column: "Catagory",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 13,
                column: "Catagory",
                value: 5);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 14,
                column: "Catagory",
                value: 4);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 15,
                column: "Catagory",
                value: 8);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 16,
                column: "Catagory",
                value: 2);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 17,
                column: "Catagory",
                value: 12);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 18,
                column: "Catagory",
                value: 4);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 19,
                column: "Catagory",
                value: 4);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 20,
                column: "Catagory",
                value: 4);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 21,
                column: "Catagory",
                value: 4);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 22,
                column: "Catagory",
                value: 5);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 23,
                column: "Catagory",
                value: 13);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 24,
                column: "Catagory",
                value: 6);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 25,
                column: "Catagory",
                value: 10);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 26,
                column: "Catagory",
                value: 6);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 27,
                column: "Catagory",
                value: 6);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 28,
                column: "Catagory",
                value: 5);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 29,
                column: "Catagory",
                value: 12);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 30,
                column: "Catagory",
                value: 3);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 31,
                column: "Catagory",
                value: 2);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 32,
                column: "Catagory",
                value: 10);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 33,
                column: "Catagory",
                value: 5);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 34,
                column: "Catagory",
                value: 6);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 35,
                column: "Catagory",
                value: 8);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 36,
                column: "Catagory",
                value: 9);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 37,
                column: "Catagory",
                value: 13);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 38,
                column: "Catagory",
                value: 8);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 39,
                column: "Catagory",
                value: 6);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 40,
                column: "Catagory",
                value: 6);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 41,
                column: "Catagory",
                value: 7);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 42,
                column: "Catagory",
                value: 5);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 43,
                column: "Catagory",
                value: 9);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 44,
                column: "Catagory",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 45,
                column: "Catagory",
                value: 10);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 46,
                column: "Catagory",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 47,
                column: "Catagory",
                value: 13);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 48,
                column: "Catagory",
                value: 6);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 49,
                column: "Catagory",
                value: 10);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 50,
                column: "Catagory",
                value: 5);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 51,
                column: "Catagory",
                value: 5);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 52,
                column: "Catagory",
                value: 13);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 53,
                column: "Catagory",
                value: 9);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 54,
                column: "Catagory",
                value: 11);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 55,
                column: "Catagory",
                value: 9);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 56,
                column: "Catagory",
                value: 5);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 57,
                column: "Catagory",
                value: 10);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 58,
                column: "Catagory",
                value: 10);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 59,
                column: "Catagory",
                value: 5);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 60,
                column: "Catagory",
                value: 10);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 61,
                column: "Catagory",
                value: 5);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 62,
                column: "Catagory",
                value: 6);

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 63,
                columns: new[] { "Catagory", "Name" },
                values: new object[] { 5, "Swimming" });

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 64,
                columns: new[] { "Catagory", "InUse", "Name" },
                values: new object[] { 10, false, "Taxes" });

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 65,
                columns: new[] { "Catagory", "InUse", "Name" },
                values: new object[] { 6, true, "Toiletries" });

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 66,
                columns: new[] { "Catagory", "Name" },
                values: new object[] { 8, "Toys" });

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 67,
                columns: new[] { "Catagory", "Name" },
                values: new object[] { 14, "Trash" });

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 68,
                columns: new[] { "Catagory", "Name" },
                values: new object[] { 14, "Travel Misc" });

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 69,
                columns: new[] { "Catagory", "Name" },
                values: new object[] { 13, "Water" });

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 70,
                columns: new[] { "Catagory", "Name" },
                values: new object[] { 5, "Weight Training" });

            migrationBuilder.UpdateData(
                table: "ExpenseCatagories",
                keyColumn: "Id",
                keyValue: 71,
                column: "Name",
                value: "Yard");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$254JJZ5mlBwTHkBjoLpmQ.b7PZyCGmaJ1KeBqbBnAIRK.DaQkpq3W");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "$2a$11$sJsy1/bq/etwSkclwEdjzev.Xw7S7z0.aVYNtIio2OOtTVVDnu9/W");
        }
    }
}
