using Microsoft.EntityFrameworkCore.Migrations;

namespace Mytheme.Migrations
{
    public partial class TemplateRenderWork : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Enabled",
                table: "Templates",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "LowerBound",
                table: "TableEntries",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UpperBound",
                table: "TableEntries",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Enabled",
                table: "TableCategories",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Enabled",
                table: "RandomTables",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "TemplateField",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TemplateForeignKey = table.Column<int>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    FieldType = table.Column<int>(nullable: false),
                    Valid = table.Column<bool>(nullable: false),
                    VariableName = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: false),
                    TemplateJson = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateField", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemplateField_Templates_TemplateForeignKey",
                        column: x => x.TemplateForeignKey,
                        principalTable: "Templates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TemplateField_TemplateForeignKey",
                table: "TemplateField",
                column: "TemplateForeignKey");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TemplateField");

            migrationBuilder.DropColumn(
                name: "Enabled",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "LowerBound",
                table: "TableEntries");

            migrationBuilder.DropColumn(
                name: "UpperBound",
                table: "TableEntries");

            migrationBuilder.DropColumn(
                name: "Enabled",
                table: "TableCategories");

            migrationBuilder.DropColumn(
                name: "Enabled",
                table: "RandomTables");
        }
    }
}
