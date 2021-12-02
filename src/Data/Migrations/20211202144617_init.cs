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
                name: "expense_main_categories",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    main_category_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_expense_main_categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "income_categories",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    category = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_income_categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "income_sources",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    source = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_income_sources", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "merchants",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    suggest_on_lookup = table.Column<bool>(type: "boolean", nullable: false),
                    city = table.Column<string>(type: "text", nullable: true),
                    state = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_merchants", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tags",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tag_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tags", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    first_name = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
                    last_name = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
                    email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "expense_sub_categories",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sub_category_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    main_categoryid = table.Column<int>(type: "integer", nullable: true),
                    in_use = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_expense_sub_categories", x => x.id);
                    table.ForeignKey(
                        name: "FK_expense_sub_categories_expense_main_categories_main_categor~",
                        column: x => x.main_categoryid,
                        principalTable: "expense_main_categories",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "incomes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    income_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    amount = table.Column<decimal>(type: "numeric", nullable: false),
                    notes = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    categoryid = table.Column<int>(type: "integer", nullable: true),
                    sourceid = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_incomes", x => x.id);
                    table.ForeignKey(
                        name: "FK_incomes_income_categories_categoryid",
                        column: x => x.categoryid,
                        principalTable: "income_categories",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_incomes_income_sources_sourceid",
                        column: x => x.sourceid,
                        principalTable: "income_sources",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "expenses",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    purchase_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    amount = table.Column<decimal>(type: "numeric", nullable: false),
                    merchantid = table.Column<int>(type: "integer", nullable: true),
                    notes = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    categoryid = table.Column<int>(type: "integer", nullable: true),
                    exclude_from_statistics = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_expenses", x => x.id);
                    table.ForeignKey(
                        name: "FK_expenses_expense_sub_categories_categoryid",
                        column: x => x.categoryid,
                        principalTable: "expense_sub_categories",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_expenses_merchants_merchantid",
                        column: x => x.merchantid,
                        principalTable: "merchants",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "expense_tags",
                columns: table => new
                {
                    expense_id = table.Column<int>(type: "integer", nullable: false),
                    tag_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_expense_tags", x => new { x.expense_id, x.tag_id });
                    table.ForeignKey(
                        name: "FK_expense_tags_expenses_expense_id",
                        column: x => x.expense_id,
                        principalTable: "expenses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_expense_tags_tags_tag_id",
                        column: x => x.tag_id,
                        principalTable: "tags",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "email", "first_name", "last_name", "password_hash" },
                values: new object[] { 1, "Mitchellscott@me.com", "Test", "User", "$2a$11$P2H/JZsVKaj9nGzOan0K9uZGKaFh6AXer8JrCYwQjrkNlXhCvhPmy" });

            migrationBuilder.CreateIndex(
                name: "IX_expense_sub_categories_main_categoryid",
                table: "expense_sub_categories",
                column: "main_categoryid");

            migrationBuilder.CreateIndex(
                name: "IX_expense_tags_tag_id",
                table: "expense_tags",
                column: "tag_id");

            migrationBuilder.CreateIndex(
                name: "IX_expenses_categoryid",
                table: "expenses",
                column: "categoryid");

            migrationBuilder.CreateIndex(
                name: "IX_expenses_merchantid",
                table: "expenses",
                column: "merchantid");

            migrationBuilder.CreateIndex(
                name: "IX_incomes_categoryid",
                table: "incomes",
                column: "categoryid");

            migrationBuilder.CreateIndex(
                name: "IX_incomes_sourceid",
                table: "incomes",
                column: "sourceid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "expense_tags");

            migrationBuilder.DropTable(
                name: "incomes");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "expenses");

            migrationBuilder.DropTable(
                name: "tags");

            migrationBuilder.DropTable(
                name: "income_categories");

            migrationBuilder.DropTable(
                name: "income_sources");

            migrationBuilder.DropTable(
                name: "expense_sub_categories");

            migrationBuilder.DropTable(
                name: "merchants");

            migrationBuilder.DropTable(
                name: "expense_main_categories");
        }
    }
}
