namespace Mytheme.Data.SQL
{
    public static class Tables
    {
        public const string Settings = "Setting";
        public const string FileData = "FileData";
        public const string TemplateCategory = "TemplateCategory";
        public const string Template = "Template";
        public const string TemplateField = "TemplateField";
        public const string TableCategory = "TableCategory";
        public const string RandomTable = "RandomTable";
        public const string TableEntry = "TableEntry";
        public const string Section = "Section";
        public const string Page = "Page";
        public const string MapPage = "MapPage";
        public const string MapMarker = "MapMarker";
        public const string Tag = "Tag";
        public const string TagMap = "TagMap";
    }

    public class TableSql
    {
        public string Settings => $@"CREATE TABLE {Tables.Settings} (
    Id Integer NOT NULL PRIMARY KEY,
    Name TEXT,
    Value TEXT);";

        public string FileData => $@"CREATE TABLE {Tables.FileData} (
    Id TEXT NOT NULL PRIMARY KEY,
    FileName TEXT,
    DisplayName TEXT, 
    FileType INTEGER)";

        //Templates

        public string TemplateCategory => $@"CREATE TABLE {Tables.TemplateCategory} (
    Id TEXT NOT NULL PRIMARY KEY,
    Name TEXT,
    Enabled INTEGER)";

        public string Templates  => $@"CREATE TABLE {Tables.Template} (
    Id TEXT NOT NULL PRIMARY KEY,
    Name TEXT,
    Category TEXT,
    Description TEXT,
    Enabled INTEGER,
    TemplateBody TEXT)";

        public string TemplateFields => $@"CREATE TABLE {Tables.TemplateField} (
    Id TEXT NOT NULL PRIMARY KEY,
    FK_Template TEXT,
    Sort INTEGER,
    FieldType INTEGER,
    Valid INTEGER,
    VariableName TEXT,
    Value TEXT,
    TemplateJson TEXT,
    FOREIGN KEY(FK_Template) REFERENCES {Tables.Template}(Id))";

        // Tables

        public string TableCategory => $@"CREATE TABLE {Tables.TableCategory} (
    Id TEXT NOT NULL PRIMARY KEY,
    Name TEXT,
    Enabled INTEGER)";

        public string RandomTables => $@"CREATE TABLE {Tables.RandomTable} (
    Id TEXT NOT NULL PRIMARY KEY,
    Name TEXT,
    Category TEXT,
    Description TEXT,
    Enabled INTEGER)";
        
        public string TableEntry => $@"CREATE TABLE {Tables.TableEntry} (
    Id TEXT NOT NULL PRIMARY KEY,
    FK_RandomTable TEXT,
    Entry TEXT,
    LowerBound INTEGER,
    UpperBound INTEGER,
    FOREIGN KEY(FK_RandomTable) REFERENCES {Tables.RandomTable}(Id))";

        // sections, pages, maps

        public string Sections => $@"CREATE TABLE {Tables.Section} (
    Id TEXT NOT NULL PRIMARY KEY,
    Parent TEXT,
    Name TEXT,
    Icon TEXT,
    Description TEXT,
    SectionType INTEGER,
    SortOrder INTEGER,
    DateCreated TEXT,
    DateModified TEXT,
    Enabled INTEGER)";

        public string Pages => $@"CREATE TABLE {Tables.Page} (
    Id TEXT NOT NULL PRIMARY KEY,
    FK_Section TEXT,
    Name TEXT,
    Content TEXT,
    DateCreated TEXT,
    DateModified TEXT,
    Enabled INTEGER,
    FOREIGN KEY(FK_Section) REFERENCES {Tables.Section}(Id))";

        public string MapPages => $@"CREATE TABLE {Tables.MapPage} (
    Id TEXT NOT NULL PRIMARY KEY,
    FK_Section TEXT,
    Name TEXT,
    Image TEXT,
    DateCreated TEXT,
    DateModified TEXT,
    Enabled INTEGER,
    FOREIGN KEY(FK_Section) REFERENCES {Tables.Section}(Id))";

        public string MapMarkers => $@"CREATE TABLE {Tables.MapMarker} (
    Id TEXT NOT NULL PRIMARY KEY,
    FK_MapPage TEXT,
    Name TEXT,
    Content TEXT,
    Lat REAL,
    Lon REAL,
    Enabled INTEGER,
    FOREIGN KEY(FK_MapPage) REFERENCES {Tables.MapPage}(Id))";

        public string Tag => $@"CREATE TABLE {Tables.Tag} (
    Id Integer NOT NULL PRIMARY KEY,
    Value TEXT);";

        public string TagMap => $@"CREATE TABLE {Tables.TagMap} (
    TagId Integer,
    PageId TEXT,
    FOREIGN KEY(TagId) REFERENCES {Tables.Tag}(Id));
    CREATE INDEX tag_id_idx ON {Tables.TagMap} (TagId);
    CREATE INDEX page_id_idx ON {Tables.TagMap} (PageId);";

    }
}
