using System;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Mytheme.Data.Dal;
using Mytheme.Data.Dto;
using Mytheme.Data.SQL;
using Serilog;

namespace Mytheme.Data
{
    // https://github.com/StackExchange/Dapper/issues/718
    public class GuidTypeHandler : SqlMapper.TypeHandler<Guid>
    {
        public override Guid Parse(object value)
        {
            return new Guid((string)value);
        }

        public override void SetValue(System.Data.IDbDataParameter parameter, Guid value)
        {
            parameter.Value = value.ToString();
        }
    }

    /// <summary>
    /// https://github.com/ericdc1/Dapper.SimpleCRUD
    /// </summary>
    public class DataStorage
    {
        private readonly string connectionString;
        private readonly string dbPath;

        public SettingDal Setting { get; }
        public FileDataDal FileData { get; }

        public TemplateCategoryDal TemplateCategory { get; }
        public TemplateDal Template { get; }
        public TemplateFieldDal TemplateField { get; }

        public TableCategoryDal TableCategory { get; }
        public RandomTableDal RandomTable { get; }
        public TableEntryDal TableEntry { get; }


        public SectionDal Section { get; }
        public PageDal Page { get; set; }
        public MapPageDal MapPage { get; }
        public MapMarkerDal MapMarker { get; }

        public DataStorage()
        {

            dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Mytheme");

            connectionString = $"Data Source={Path.Combine(dbPath, "mytheme.sqlite")}";

            Setting = new SettingDal(connectionString);
            FileData = new FileDataDal(connectionString);

            TemplateCategory = new TemplateCategoryDal(connectionString);
            Template = new TemplateDal(connectionString);
            TemplateField = new TemplateFieldDal(connectionString);

            TableCategory = new TableCategoryDal(connectionString);
            RandomTable = new RandomTableDal(connectionString);
            TableEntry = new TableEntryDal(connectionString);

            Section = new SectionDal(connectionString);
            Page = new PageDal(connectionString);
            MapPage = new MapPageDal(connectionString);
            MapMarker = new MapMarkerDal(connectionString);
        }

        public async Task MigrateDatabase()
        {
            if (!File.Exists(Path.Combine(dbPath, "mytheme.sqlite")))
            {
                await InitializeDb();
            }
        }


        public async Task<Section[]> GetAllCampaigns()
        {
            var sql = $"SELECT * FROM Section WHERE SectionType == {(int) SectionType.Campaign}";
            await using var conn = SimpleDbConnection();
            await conn.OpenAsync();
            var result = conn.Query<Section>(sql).ToArray();
            await conn.CloseAsync();
            return result;
        }


        private async Task InitializeDb()
        {
            try
            {
                var tables = new TableSql();

                await using var conn = SimpleDbConnection();

                await conn.OpenAsync();

                Log.Debug("Create Settings Table");
                await conn.ExecuteAsync(tables.Settings);
                Log.Debug("Create FileData Table");
                await conn.ExecuteAsync(tables.FileData);
                Log.Debug("Create TemplateCategory Table");
                await conn.ExecuteAsync(tables.TemplateCategory);
                Log.Debug("Create Template Table");
                await conn.ExecuteAsync(tables.Templates);
                Log.Debug("Create TemplateFields Table");
                await conn.ExecuteAsync(tables.TemplateFields);
                Log.Debug("Create TableCategory Table");
                await conn.ExecuteAsync(tables.TableCategory);
                Log.Debug("Create RandomTable Table");
                await conn.ExecuteAsync(tables.RandomTables);
                Log.Debug("Create TableEntry Table");
                await conn.ExecuteAsync(tables.TableEntry);
                Log.Debug("Create Section Table");
                await conn.ExecuteAsync(tables.Sections);
                Log.Debug("Create Page Table");
                await conn.ExecuteAsync(tables.Pages);
                Log.Debug("Create MapPage Table");
                await conn.ExecuteAsync(tables.MapPages);
                Log.Debug("Create MapMarker Table");
                await conn.ExecuteAsync(tables.MapMarkers);
                Log.Debug("Create Tag Table");
                await conn.ExecuteAsync(tables.Tag);
                Log.Debug("Create TagMap Table");
                await conn.ExecuteAsync(tables.TagMap);

                Log.Debug("Settings DB version to 1");
                var version = new Setting {Name = "version", Value = "1"};

                await conn.InsertAsync(version);

                await conn.CloseAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Exception initializing database!");
                throw;
            }
        }

        private SQLiteConnection SimpleDbConnection()
        {
            return new SQLiteConnection(connectionString);
        }
    }
}
