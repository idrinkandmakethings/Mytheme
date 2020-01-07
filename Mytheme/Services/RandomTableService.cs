using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mytheme.Data;
using Mytheme.Data.Dto;
using Mytheme.Services.Interfaces;
using Serilog;

namespace Mytheme.Services
{
    public class RandomTableService : IRandomTableService
    {
        private readonly DataStorage db;

        public RandomTableService(DataStorage db)
        {
            this.db = db;
        }

        public async Task<DalResult<Guid>> AddRandomTable(RandomTable table)
        {
            try
            {
                var id = await db.RandomTable.InsertAsync(table);

                return new DalResult<Guid>(DalStatus.Success, id);
            }
            catch (Exception e)
            {
                Log.Error(e,"Exception saving table {Name}.", table.Name);
                return new DalResult<Guid>(DalStatus.Unknown, Guid.Empty, "Error saving table");
            }
        }

        public async Task<DalResult> UpdateRandomTable(RandomTable table)
        {
            try
            {
                var result = await db.RandomTable.UpdateAsync(table);

                return new DalResult(result ? DalStatus.Success : DalStatus.Unknown);
            }
            catch (Exception e)
            {
                Log.Error(e, "Exception updating table id {Id}.", table.Id);
                return new DalResult(DalStatus.Unknown, "Error updating table");
            }
        }

        public async Task<DalResult<RandomTable>> GetRandomTable(Guid id)
        {
            try
            {
                var result = await db.RandomTable.GetAsync(id);
                result.Entries = await db.TableEntry.GetByTableIdAsync(result.Id);
                return new DalResult<RandomTable>(DalStatus.Success, result);
            }
            catch (Exception e)
            {
                Log.Error(e, "Exception getting table id {Id}.", id);
                return new DalResult<RandomTable>(DalStatus.Unknown, null, "Unknown error retrieving table");
            }
        }

        public async Task<DalResult<RandomTable>> GetRandomTableByName(string name)
        {
            try
            {
                var result = await db.RandomTable.GetByNameAsync(name);
                result.Entries = await db.TableEntry.GetByTableIdAsync(result.Id);
                return new DalResult<RandomTable>(DalStatus.Success, result);
            }
            catch (Exception e)
            {
                Log.Error(e,"Exception getting table name {Name}.", name);
                return new DalResult<RandomTable>(DalStatus.Unknown, null, "Unknown error retrieving table");
            }
        }

        public async Task<DalResult<RandomTable[]>> GetAllRandomTables()
        {
            try
            {
                var result = await db.RandomTable.GetAllAsync();
                return new DalResult<RandomTable[]>(DalStatus.Success, result);
            }
            catch (Exception e)
            {
                Log.Error(e,"Exception getting tables.");
                return new DalResult<RandomTable[]>(DalStatus.Unknown, null, "Unknown error retrieving tables");
            }
        }

        public async Task<DalResult<List<string>>> GetCategories()
        {
            try
            {
                var result = await db.TableCategory.GetAllAsync();
                return new DalResult<List<string>>(DalStatus.Success, result.Select(x => x.Name).ToList());
            }
            catch (Exception e)
            {
                Log.Error(e, $"Exception getting Table Categories.");
                return new DalResult<List<string>>(DalStatus.Unknown, null, "Unknown error retrieving categories");
            }
        }

        public async Task<DalResult> AddCategory(string category)
        {
            try
            {
                _ = await db.TableCategory.InsertAsync(new TableCategory() {Name = category, Enabled = true});
                return new DalResult(DalStatus.Success);
            }
            catch (Exception e)
            {
                Log.Error(e, "Exception adding Table Category {Category}", category);
                return new DalResult(DalStatus.Unknown, "Error saving table");
            }
        }

        public async Task<DalResult<bool>> CategoryExists(string name)
        {

            try
            {
                var exists = await db.TableCategory.Exists(name);
                return new DalResult<bool>(DalStatus.Success, exists);
            }
            catch (Exception e)
            {
                Log.Error(e,"Exception checking if Table Category {Name} exists.", name);
                return new DalResult<bool>(DalStatus.Unknown, false, "Error determining category exists");
            }
        }

        public async Task<DalResult<bool>> TableExists(string name)
        {
            try
            {
                var exists = await db.RandomTable.Exists(name);
                return new DalResult<bool>(DalStatus.Success, exists);
            }
            catch (Exception e)
            {
                Log.Error(e, "Exception checking if table {name} exists", name);
                return new DalResult<bool>(DalStatus.Unknown, false, "Error determining table exists");
            }
        }
    }
}
