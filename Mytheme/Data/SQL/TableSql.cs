namespace Mytheme.Data.SQL
{
    public static class Tables
    {
        public const string Settings = "Setting";
        public const string FileData = "FileData";
        public const string TemplateCategory = "TempalateCategory";
        public const string Template = "Template";
        public const string TemplateField = "TemplateField";
        public const string TableCategory = "TableCategory";
        public const string RandomTable = "RandomTable";
        public const string TableEntry = "TableEntry";
        public const string Section = "Section";
        public const string Page = "Page";
        public const string MapPage = "MapPage";
        public const string MapMarker = "MapMarker";
    }

    public class TableSql
    {
        public string Settings => $@"CREATE TABLE {Tables.Settings} (
    Id TEXT NOT NULL NOT NULL PRIMARY KEY,
    Value TEXT);";

        public string FileData => $@"CREATE TABLE {Tables.FileData} (
    Id BLOB NOT NULL PRIMARY KEY,
    FileName TEXT,
    DisplayName TEXT, 
    FileType INTEGER)";

        //Templates

        public string TemplateCategory => $@"CREATE TABLE {Tables.TemplateCategory} (
    Id BLOB NOT NULL PRIMARY KEY,
    Name TEXT,
    Enabled INTEGER)";

        public string Templates  => $@"CREATE TABLE {Tables.Template} (
    Id BLOB NOT NULL PRIMARY KEY,
    Name TEXT,
    Category TEXT,
    Description TEXT,
    Enabled INTEGER,
    TemplateBody TEXT)";

        public string TemplateFields => $@"CREATE TABLE {Tables.TemplateField} (
    Id BLOB NOT NULL PRIMARY KEY,
    FK_Template BLOB,
    Sort INTEGER,
    FieldType INTEGER,
    Valid INTEGER,
    VariableName TEXT,
    Value TEXT,
    TemplateJson TEXT,
    FOREIGN KEY(FK_Template) REFERENCES Template(Id))";

        // Tables

        public string TableCategory => $@"CREATE TABLE {Tables.TableCategory} (
    Id BLOB NOT NULL PRIMARY KEY,
    Name TEXT,
    Enabled INTEGER)";

        public string RandomTables => $@"CREATE TABLE {Tables.RandomTable} (
    Id BLOB NOT NULL PRIMARY KEY,
    Name TEXT,
    Category TEXT,
    Description TEXT,
    Enabled INTEGER)";
        
        public string TableEntry => $@"CREATE TABLE {Tables.TableEntry} (
    Id BLOB NOT NULL PRIMARY KEY,
    FK_RandomTable BLOB,
    Entry TEXT,
    LowerBound INTEGER,
    UpperBound INTEGER,
    FOREIGN KEY(FK_RandomTable) REFERENCES RandomTable(Id))";

        // sections, pages, maps

        public string Sections => $@"CREATE TABLE {Tables.Section} (
    Id BLOB NOT NULL PRIMARY KEY,
    Parent BLOB,
    Name TEXT,
    Icon TEXT,
    Description TEXT,
    SectionType INTEGER,
    SortOrder INTEGER,
    DateCreated TEXT,
    DateModified TEXT,
    Enabled INTEGER)";

        public string Pages => $@"CREATE TABLE {Tables.Page} (
    Id BLOB NOT NULL PRIMARY KEY,
    FK_Section BLOB,
    Name TEXT,
    PageType INTEGER,
    Link TEXT,
    Content TEXT,
    DateCreated TEXT,
    DateModified TEXT,
    Enabled INTEGER,
    FOREIGN KEY(FK_Section) REFERENCES Section(Id))";

        public string MapPages => $@"CREATE TABLE {Tables.MapPage} (
    Id BLOB NOT NULL PRIMARY KEY,
    FK_Section BLOB,
    Name TEXT,
    Link TEXT,
    Image TEXT,
    DateCreated TEXT,
    DateModified TEXT,
    Enabled INTEGER,
    FOREIGN KEY(FK_Section) REFERENCES Section(Id))";

        public string MapMarkers => $@"CREATE TABLE {Tables.MapMarker} (
    Id BLOB NOT NULL PRIMARY KEY,
    FK_MapPage BLOB,
    Name TEXT,
    Content TEXT,
    Lat REAL,
    Lon REAL,
    Enabled INTEGER,
    FOREIGN KEY(FK_MapPage) REFERENCES MapPage(Id))";
    }
}
