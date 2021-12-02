using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CashTrack.Data.Migrations
{
    public partial class testinit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "last_name",
                table: "users",
                type: "character varying(25)",
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "first_name",
                table: "users",
                type: "character varying(25)",
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                column: "password_hash",
                value: "$2a$11$/cGxSeiH8yGFTG/t4BpEDOzcGPB3K9kCyN6n7bz7hJWloN0kxlfci");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 2,
                column: "password_hash",
                value: "$2a$11$fuOlOr3nrtXE9WpxqU9k5.0pnV474rd9V3DCE23iwaS0GFH9yzmOe");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "last_name",
                table: "users",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(25)",
                oldMaxLength: 25,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "first_name",
                table: "users",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(25)",
                oldMaxLength: 25,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                column: "password_hash",
                value: "$2a$11$FEkIeLfunFg69jX36cK90eQ4YsJgp2O/N1fXUFrhWewk4looXtdkG");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 2,
                column: "password_hash",
                value: "$2a$11$c.5mAP83IB30EFPYuxrfMuWSEEXFKTlBKaufiMpRW0YdrSslltV2u");
        }
    }
}
