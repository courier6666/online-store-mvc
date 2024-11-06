using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Persistence.Main.Migrations
{
    public partial class AddedProductArchivedProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRemovedFromPageStore",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRemovedFromPageStore",
                table: "Products");
        }
    }
}
