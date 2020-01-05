using System;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DapperExtensions;
using Mytheme.Data.Dal;
using Mytheme.Data.Dto;
using Mytheme.Data.SQL;
using Serilog;

namespace Mytheme.Data
{
    public class DataStorage
    {
        private readonly string connectionString;
        private readonly string dbPath;

        public FileDataDal FileData { get; }

        public DataStorage()
        {

            dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Mytheme");

            connectionString = $"Data Source={Path.Combine(dbPath, "mytheme.sqlite")}";

            FileData = new FileDataDal(connectionString);
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

                Log.Debug("Settings DB version to 1");
                var version = new Setting {Id = "version", Value = "1"};

                conn.Insert(version);

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
