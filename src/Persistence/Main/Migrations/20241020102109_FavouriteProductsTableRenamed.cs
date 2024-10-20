using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Persistence.Main.Migrations
{
    public partial class FavouriteProductsTableRenamed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavouriteProduct_Products_ProductId",
                table: "FavouriteProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_FavouriteProduct_Users_UserId",
                table: "FavouriteProduct");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FavouriteProduct",
                table: "FavouriteProduct");

            migrationBuilder.RenameTable(
                name: "FavouriteProduct",
                newName: "FavoruiteProducts");

            migrationBuilder.RenameIndex(
                name: "IX_FavouriteProduct_ProductId",
                table: "FavoruiteProducts",
                newName: "IX_FavoruiteProducts_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FavoruiteProducts",
                table: "FavoruiteProducts",
                columns: new[] { "UserId", "ProductId" });

            migrationBuilder.AddForeignKey(
                name: "FK_FavoruiteProducts_Products_ProductId",
                table: "FavoruiteProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FavoruiteProducts_Users_UserId",
                table: "FavoruiteProducts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoruiteProducts_Products_ProductId",
                table: "FavoruiteProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_FavoruiteProducts_Users_UserId",
                table: "FavoruiteProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FavoruiteProducts",
                table: "FavoruiteProducts");

            migrationBuilder.RenameTable(
                name: "FavoruiteProducts",
                newName: "FavouriteProduct");

            migrationBuilder.RenameIndex(
                name: "IX_FavoruiteProducts_ProductId",
                table: "FavouriteProduct",
                newName: "IX_FavouriteProduct_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FavouriteProduct",
                table: "FavouriteProduct",
                columns: new[] { "UserId", "ProductId" });

            migrationBuilder.AddForeignKey(
                name: "FK_FavouriteProduct_Products_ProductId",
                table: "FavouriteProduct",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FavouriteProduct_Users_UserId",
                table: "FavouriteProduct",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
