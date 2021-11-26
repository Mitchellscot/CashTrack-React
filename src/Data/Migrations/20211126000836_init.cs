using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CashTrack.Data.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "expense-main-categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Category = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_expense-main-categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "income-categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Category = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_income-categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "income-sources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Source = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_income-sources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "merchants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    SuggestOnLookup = table.Column<bool>(type: "boolean", nullable: false),
                    City = table.Column<string>(type: "text", nullable: true),
                    State = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_merchants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TagName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "expense-sub-categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: true),
                    InUse = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_expense-sub-categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_expense-sub-categories_expense-main-categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "expense-main-categories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "incomes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IncomeDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Notes = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CategoryId = table.Column<int>(type: "integer", nullable: true),
                    SourceId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_incomes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_incomes_income-categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "income-categories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_incomes_income-sources_SourceId",
                        column: x => x.SourceId,
                        principalTable: "income-sources",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "expenses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PurchaseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    MerchantId = table.Column<int>(type: "integer", nullable: true),
                    Notes = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CategoryId = table.Column<int>(type: "integer", nullable: true),
                    ExcludeFromStatistics = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_expenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_expenses_expense-sub-categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "expense-sub-categories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_expenses_merchants_MerchantId",
                        column: x => x.MerchantId,
                        principalTable: "merchants",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "expense-tags",
                columns: table => new
                {
                    ExpenseId = table.Column<int>(type: "integer", nullable: false),
                    TagId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_expense-tags", x => new { x.ExpenseId, x.TagId });
                    table.ForeignKey(
                        name: "FK_expense-tags_expenses_ExpenseId",
                        column: x => x.ExpenseId,
                        principalTable: "expenses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_expense-tags_tags_TagId",
                        column: x => x.TagId,
                        principalTable: "tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "PasswordHash" },
                values: new object[,]
                {
                    { 1, "Mitchellscott@me.com", "Mitchell", "Scott", "$2a$11$36C19UjjGPuk5R25MR6DouUx2WcQ7LMoQhdQQYBIgUoEv8tL/pbkW" },
                    { 2, "Sarahlscott@me.com", "Sarah", "Scott", "$2a$11$iOtc3zz6D1lIZ6Vl7AfgB.4ZhRi4SOogozOQHe3xx9iAYFkCMGUre" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_expense-sub-categories_CategoryId",
                table: "expense-sub-categories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_expense-tags_TagId",
                table: "expense-tags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_expenses_CategoryId",
                table: "expenses",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_expenses_MerchantId",
                table: "expenses",
                column: "MerchantId");

            migrationBuilder.CreateIndex(
                name: "IX_incomes_CategoryId",
                table: "incomes",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_incomes_SourceId",
                table: "incomes",
                column: "SourceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "expense-tags");

            migrationBuilder.DropTable(
                name: "incomes");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "expenses");

            migrationBuilder.DropTable(
                name: "tags");

            migrationBuilder.DropTable(
                name: "income-categories");

            migrationBuilder.DropTable(
                name: "income-sources");

            migrationBuilder.DropTable(
                name: "expense-sub-categories");

            migrationBuilder.DropTable(
                name: "merchants");

            migrationBuilder.DropTable(
                name: "expense-main-categories");
        }
    }
}
