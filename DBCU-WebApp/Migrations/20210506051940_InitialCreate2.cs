using Microsoft.EntityFrameworkCore.Migrations;

namespace DBCU_WebApp.Migrations
{
    public partial class InitialCreate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SlugEn",
                table: "News",
                maxLength: 160,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StrUrlImage",
                table: "News",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SlugEn",
                table: "News");

            migrationBuilder.DropColumn(
                name: "StrUrlImage",
                table: "News");
        }
    }
}
