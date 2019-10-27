using Microsoft.EntityFrameworkCore.Migrations;

namespace Mytheme.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RandomTables",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: false),
                    Category = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RandomTables", x => x.Id);
                    table.UniqueConstraint("AK_RandomTables_Name", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "TableCategories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TableCategories", x => x.Id);
                    table.UniqueConstraint("AK_TableCategories_Name", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "TableEntries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RandomTableForeignKey = table.Column<int>(nullable: false),
                    Entry = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TableEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TableEntries_RandomTables_RandomTableForeignKey",
                        column: x => x.RandomTableForeignKey,
                        principalTable: "RandomTables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TableEntries_RandomTableForeignKey",
                table: "TableEntries",
                column: "RandomTableForeignKey");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TableCategories");

            migrationBuilder.DropTable(
                name: "TableEntries");

            migrationBuilder.DropTable(
                name: "RandomTables");
        }
    }
}
