using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace CashTrack.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExpenseCatagories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Catagory = table.Column<int>(type: "integer", nullable: false),
                    InUse = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseCatagories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Income",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IncomeDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Notes = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Catagory = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Income", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    PasswordHash = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Expenses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PurchaseDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Merchant = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Notes = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CatagoryId = table.Column<int>(type: "integer", nullable: true),
                    Tags = table.Column<List<string>>(type: "text[]", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Expenses_ExpenseCatagories_CatagoryId",
                        column: x => x.CatagoryId,
                        principalTable: "ExpenseCatagories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "ExpenseCatagories",
                columns: new[] { "Id", "Catagory", "InUse", "Name" },
                values: new object[,]
                {
                    { 1, 7, true, "AAA" },
                    { 52, 13, true, "Phone" },
                    { 51, 5, true, "Pet" },
                    { 50, 5, true, "Outdoor" },
                    { 49, 10, true, "Other" },
                    { 48, 6, true, "Office Supplies" },
                    { 47, 13, true, "Natural Gas" },
                    { 53, 9, true, "Property Taxes" },
                    { 46, 1, true, "Music" },
                    { 44, 1, true, "Movies" },
                    { 43, 9, true, "Mortgage" },
                    { 42, 5, true, "Mandolin" },
                    { 41, 7, true, "Life Insurance" },
                    { 40, 6, false, "Laundry" },
                    { 39, 6, true, "Kitchen & Bath" },
                    { 45, 10, false, "Moving Expenses" },
                    { 38, 8, true, "Kid Related" },
                    { 54, 11, true, "Reimbursement" },
                    { 56, 5, true, "Running" },
                    { 70, 6, true, "Yard" },
                    { 69, 5, true, "Weight Training" },
                    { 68, 13, true, "Water" },
                    { 67, 14, true, "Travel Misc" },
                    { 66, 8, true, "Toys" },
                    { 65, 6, true, "Toiletries" },
                    { 55, 9, true, "Rent" },
                    { 64, 10, true, "Taxes" },
                    { 62, 6, true, "Software" },
                    { 61, 5, true, "Shooting" },
                    { 60, 10, true, "Shipping" },
                    { 59, 5, true, "Sewing" },
                    { 58, 10, true, "Seasonal" },
                    { 57, 10, true, "School" },
                    { 63, 5, true, "Swimming" },
                    { 37, 13, true, "Internet" },
                    { 36, 9, true, "Homeowners Ins." },
                    { 35, 8, true, "Home School" },
                    { 15, 8, true, "Diapers" },
                    { 14, 4, true, "Dentist" },
                    { 13, 5, true, "Cycling" },
                    { 12, 0, true, "Clothing (Sarah)" },
                    { 11, 0, true, "Clothing (Mitch)" },
                    { 10, 0, true, "Clothing (Kids)" },
                    { 16, 2, true, "Dining Out" },
                    { 9, 3, true, "Church" },
                    { 7, 12, true, "Car Stuff" },
                    { 6, 12, true, "Car Repairs" },
                    { 5, 12, true, "Car Insurance" },
                    { 4, 1, true, "Books" },
                    { 3, 5, false, "Banjo" },
                    { 2, 14, true, "Airfare" },
                    { 8, 12, true, "Car Wash" },
                    { 17, 12, true, "DMV" },
                    { 18, 4, true, "Doctor (Kids)" },
                    { 19, 4, true, "Doctor (Mitch)" },
                    { 34, 6, true, "Home Repairs" },
                    { 33, 5, true, "Hobbies" },
                    { 32, 10, true, "Haircut" },
                    { 31, 2, true, "Groceries" },
                    { 30, 3, true, "Gifts" },
                    { 29, 12, true, "Gas" },
                    { 28, 5, true, "Garden" },
                    { 27, 6, true, "Games" },
                    { 26, 6, true, "Furniture" },
                    { 25, 10, true, "Fees" },
                    { 24, 6, true, "Electronics" },
                    { 23, 13, true, "Electricity" },
                    { 22, 5, true, "Drums" },
                    { 21, 4, true, "Drugs" },
                    { 20, 4, true, "Doctor (Sarah)" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "PasswordHash" },
                values: new object[,]
                {
                    { 1, "Mitchellscott@me.com", "Mitchell", "Scott", "$2a$11$RmSj/BMT4GGSYOYSInaeUuUZBjpj8ccCx9A9eZTfevQARdXjPte76" },
                    { 2, "Sarahscott@me.com", "Sarah", "Scott", "$2a$11$Mp5xTm3Xf.b1gFn3qv0ppOhhRmEgsaxJH/jO.ue/DSxuXRtPp5TDi" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_CatagoryId",
                table: "Expenses",
                column: "CatagoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Expenses");

            migrationBuilder.DropTable(
                name: "Income");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "ExpenseCatagories");
        }
    }
}
