using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mytheme.Data;
using Mytheme.Data.Dto;
using Mytheme.Services.Interfaces;
using Serilog;

namespace Mytheme.Services
{
    public class RandomTableService : IRandomTableService
    {
        public Task<DalResult<Guid>> AddRandomTable(RandomTable table)
        {
            throw new NotImplementedException();
        }

        public Task<DalResult> UpdateRandomTable(RandomTable table)
        {
            throw new NotImplementedException();
        }

        public Task<DalResult<RandomTable>> GetRandomTable(string id)
        {
            throw new NotImplementedException();
        }

        public Task<DalResult<RandomTable>> GetRandomTableByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<DalResult<RandomTable[]>> GetAllRandomTables()
        {
            throw new NotImplementedException();
        }

        public Task<DalResult<List<string>>> GetCategories()
        {
            throw new NotImplementedException();
        }

        public Task<DalResult> AddCategory(string category)
        {
            throw new NotImplementedException();
        }

        public Task<DalResult<bool>> CategoryExists(string name)
        {
            throw new NotImplementedException();
        }

        public Task<DalResult<bool>> TableExists(string name)
        {
            throw new NotImplementedException();
        }
    }
}

//        private readonly DataStorage db;

//        public RandomTableService(DataStorage db)
//        {
//            this.db = db;
//        }

//        public async Task<DalResult> AddRandomTable(RandomTable table)
//        {
//            return await Task.Run(async () =>
//            {
//                try
//                {
//                    var result = await db.RandomTables.AddAsync(table);
//                    db.SaveChanges(true);

//                    return new DalResult(DalStatus.Success);
//                }
//                catch (Exception e)
//                {
//                    Log.Error($"Exception saving table {table.Name}. ex: {e.Message}");
//                    Log.Debug(e.StackTrace);
//                    return new DalResult(DalStatus.Unknown, "Error saving table");
//                }
//            });
//        }

//        public async Task<DalResult> UpdateRandomTable(RandomTable table)
//        {
//            return await Task.Run(() =>
//            {
//                try
//                {
//                    db.RandomTables.Update(table);
//                    db.SaveChanges(true);

//                    return new DalResult(DalStatus.Success);
//                }
//                catch (Exception e)
//                {
//                    Log.Error($"Exception updating table id {table.Id}. ex: {e.Message}");
//                    Log.Debug(e.StackTrace);
//                    return new DalResult(DalStatus.Unknown, "Error updating table");
//                }
//            });
//        }

//        public async Task<DalResult<RandomTable>> GetRandomTable(string id)
//        {
//            return await Task.Run(() =>
//            {
//                try
//                {
//                    var result = db.RandomTables
//                        .Include(t => t.Entries)
//                        .First(t => t.Id == id);
//                    return new DalResult<RandomTable>(DalStatus.Success, result);

//                }
//                catch (Exception e)
//                {
//                    Log.Error($"Exception getting table id {id}. ex: {e.Message}");
//                    Log.Debug(e.StackTrace);
//                    return new DalResult<RandomTable>(DalStatus.Unknown, null, "Unknown error retrieving table");
//                }
//            });
//        }

//        public async Task<DalResult<RandomTable>> GetRandomTableByName(string name)
//        {
//            return await Task.Run(() =>
//            {
//                try
//                {
//                    var result = db.RandomTables
//                        .Include(t => t.Entries)
//                        .First(t => t.Name == name);
//                    return new DalResult<RandomTable>(DalStatus.Success, result);
//                }
//                catch (Exception e)
//                {
//                    Log.Error($"Exception getting table name {name}. ex: {e.Message}");
//                    Log.Debug(e.StackTrace);
//                    return new DalResult<RandomTable>(DalStatus.Unknown, null, "Unknown error retrieving table");
//                }
//            });
//        }

//        public async Task<DalResult<RandomTable[]>> GetAllRandomTables()
//        {
//            return await Task.Run(() =>
//            {
//                try
//                {
//                    var result = db.RandomTables.ToArray();
//                    return new DalResult<RandomTable[]>(DalStatus.Success, result);
//                }
//                catch (Exception e)
//                {
//                    Log.Error($"Exception getting tables. ex: {e.Message}");
//                    Log.Debug(e.StackTrace);
//                    return new DalResult<RandomTable[]>(DalStatus.Unknown, null, "Unknown error retrieving tables");
//                }
//            });
//        }

//        public async Task<DalResult<List<string>>> GetCategories()
//        {
//            return await Task.Run(() =>
//            {
//                try
//                {
//                    var result = db.TableCategories.Select(x => x.Name).OrderBy(n => n).ToList();
//                    return new DalResult<List<string>>(DalStatus.Success, result);
//                }
//                catch (Exception e)
//                {
//                    Log.Error($"Exception getting Table Categories. ex: {e.Message}");
//                    Log.Debug(e.StackTrace);
//                    return new DalResult<List<string>>(DalStatus.Unknown, null, "Unknown error retrieving categories");
//                }
//            });
//        }

//        public async Task<DalResult> AddCategory(string category)
//        {
//            return await Task.Run(async () =>
//            {
//                try
//                {
//                    var result = await db.TableCategories.AddAsync(new TableCategory() {Name = category});
//                    db.SaveChanges(true);
//                    return new DalResult(DalStatus.Success);
//                }
//                catch (Exception e)
//                {
//                    Log.Error($"Exception adding Table Category {category}. ex: {e.Message}");
//                    Log.Debug(e.StackTrace);
//                    return new DalResult(DalStatus.Unknown, "Error saving table");
//                }
//            });
//        }

//        public async Task<DalResult<bool>> CategoryExists(string name)
//        {
//            return await Task.Run(() =>
//            {
//                try
//                {
//                    var exists = db.TableCategories.ToList().Any(x => x.Name == name);
//                    return new DalResult<bool>(DalStatus.Success, exists);
//                }
//                catch (Exception e)
//                {
//                    Log.Error($"Exception checking if Table Category {name} exists. ex: {e.Message}");
//                    Log.Debug(e.StackTrace);
//                    return new DalResult<bool>(DalStatus.Unknown, false, "Error determining category exists");
//                }
//            });
//        }

//        public async Task<DalResult<bool>> TableExists(string name)
//        {
//            return await Task.Run(() =>
//            {
//                try
//                {
//                    var exists = db.RandomTables.ToList().Any(x => x.Name == name);
//                    return new DalResult<bool>(DalStatus.Success, exists);
//                }
//                catch (Exception e)
//                {
//                    Log.Error($"Exception checking if table {name} exists. ex: {e.Message}");
//                    Log.Debug(e.StackTrace);
//                    return new DalResult<bool>(DalStatus.Unknown, false, "Error determining table exists");
//                }
//            });
//        }
//    }
//}
