using Microsoft.EntityFrameworkCore.Migrations;

namespace Mytheme.Migrations
{
    public partial class AddPageType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PageType",
                table: "Pages",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PageType",
                table: "Pages");
        }
    }
}
