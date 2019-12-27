using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mytheme.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FileData",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    FileName = table.Column<string>(nullable: false),
                    DisplayName = table.Column<string>(nullable: false),
                    FileType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RandomTables",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Category = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Enabled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RandomTables", x => x.Id);
                    table.UniqueConstraint("AK_RandomTables_Name", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Sections",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Parent = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    SectionType = table.Column<int>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    Icon = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    Enabled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TableCategories",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Enabled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TableCategories", x => x.Id);
                    table.UniqueConstraint("AK_TableCategories_Name", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "TemplateCategories",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateCategories", x => x.Id);
                    table.UniqueConstraint("AK_TemplateCategories_Name", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Templates",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Category = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Enabled = table.Column<bool>(nullable: false),
                    TemplateBody = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Templates", x => x.Id);
                    table.UniqueConstraint("AK_Templates_Name", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "TableEntry",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FK_RandomTable = table.Column<string>(nullable: true),
                    Entry = table.Column<string>(nullable: true),
                    LowerBound = table.Column<int>(nullable: false),
                    UpperBound = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TableEntry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TableEntry_RandomTables_FK_RandomTable",
                        column: x => x.FK_RandomTable,
                        principalTable: "RandomTables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MapPages",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    FK_Section = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Link = table.Column<string>(nullable: false),
                    Image = table.Column<string>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    Enabled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapPages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MapPages_Sections_FK_Section",
                        column: x => x.FK_Section,
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pages",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    FK_Section = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    PageType = table.Column<int>(nullable: false),
                    Link = table.Column<string>(nullable: false),
                    Content = table.Column<string>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    Enabled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pages_Sections_FK_Section",
                        column: x => x.FK_Section,
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TemplateField",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FK_Template = table.Column<string>(nullable: true),
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
                        name: "FK_TemplateField_Templates_FK_Template",
                        column: x => x.FK_Template,
                        principalTable: "Templates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MapMarkers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    FK_MapPage = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Content = table.Column<string>(nullable: false),
                    Lat = table.Column<double>(nullable: false),
                    Lon = table.Column<double>(nullable: false),
                    Enabled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapMarkers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MapMarkers_MapPages_FK_MapPage",
                        column: x => x.FK_MapPage,
                        principalTable: "MapPages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MapMarkers_FK_MapPage",
                table: "MapMarkers",
                column: "FK_MapPage");

            migrationBuilder.CreateIndex(
                name: "IX_MapPages_FK_Section",
                table: "MapPages",
                column: "FK_Section");

            migrationBuilder.CreateIndex(
                name: "IX_Pages_FK_Section",
                table: "Pages",
                column: "FK_Section");

            migrationBuilder.CreateIndex(
                name: "IX_TableEntry_FK_RandomTable",
                table: "TableEntry",
                column: "FK_RandomTable");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateField_FK_Template",
                table: "TemplateField",
                column: "FK_Template");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileData");

            migrationBuilder.DropTable(
                name: "MapMarkers");

            migrationBuilder.DropTable(
                name: "Pages");

            migrationBuilder.DropTable(
                name: "TableCategories");

            migrationBuilder.DropTable(
                name: "TableEntry");

            migrationBuilder.DropTable(
                name: "TemplateCategories");

            migrationBuilder.DropTable(
                name: "TemplateField");

            migrationBuilder.DropTable(
                name: "MapPages");

            migrationBuilder.DropTable(
                name: "RandomTables");

            migrationBuilder.DropTable(
                name: "Templates");

            migrationBuilder.DropTable(
                name: "Sections");
        }
    }
}
