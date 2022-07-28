using Microsoft.EntityFrameworkCore.Migrations;

namespace DBCU_WebApp.Migrations
{
    public partial class InitialCreate4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostCategories_Category_CategoryID",
                table: "PostCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_PostCategories_News_PostID",
                table: "PostCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostCategories",
                table: "PostCategories");

            migrationBuilder.RenameTable(
                name: "PostCategories",
                newName: "NewsCategorys");

            migrationBuilder.RenameIndex(
                name: "IX_PostCategories_PostID",
                table: "NewsCategorys",
                newName: "IX_NewsCategorys_PostID");

            migrationBuilder.RenameIndex(
                name: "IX_PostCategories_CategoryID",
                table: "NewsCategorys",
                newName: "IX_NewsCategorys_CategoryID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NewsCategorys",
                table: "NewsCategorys",
                columns: new[] { "NewsID", "CategoryID" });

            migrationBuilder.AddForeignKey(
                name: "FK_NewsCategorys_Category_CategoryID",
                table: "NewsCategorys",
                column: "CategoryID",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NewsCategorys_News_PostID",
                table: "NewsCategorys",
                column: "PostID",
                principalTable: "News",
                principalColumn: "NewsId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewsCategorys_Category_CategoryID",
                table: "NewsCategorys");

            migrationBuilder.DropForeignKey(
                name: "FK_NewsCategorys_News_PostID",
                table: "NewsCategorys");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NewsCategorys",
                table: "NewsCategorys");

            migrationBuilder.RenameTable(
                name: "NewsCategorys",
                newName: "PostCategories");

            migrationBuilder.RenameIndex(
                name: "IX_NewsCategorys_PostID",
                table: "PostCategories",
                newName: "IX_PostCategories_PostID");

            migrationBuilder.RenameIndex(
                name: "IX_NewsCategorys_CategoryID",
                table: "PostCategories",
                newName: "IX_PostCategories_CategoryID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostCategories",
                table: "PostCategories",
                columns: new[] { "NewsID", "CategoryID" });

            migrationBuilder.AddForeignKey(
                name: "FK_PostCategories_Category_CategoryID",
                table: "PostCategories",
                column: "CategoryID",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostCategories_News_PostID",
                table: "PostCategories",
                column: "PostID",
                principalTable: "News",
                principalColumn: "NewsId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
