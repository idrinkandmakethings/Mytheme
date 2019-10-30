using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mytheme.Dal;
using Mytheme.Dal.Dto;
using Mytheme.Data.Interfaces;
using Serilog;

namespace Mytheme.Data
{
    public class RandomTableService : IRandomTableService
    {
        public async Task<DalResult> AddRandomTable(RandomTable table)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    await using var db = new DataStorage();
                    var result = await db.RandomTables.AddAsync(table);
                    db.SaveChanges(true);

                    return new DalResult(DalStatus.Success);
                }
                catch (Exception e)
                {
                    Log.Error($"Exception saving table {table.Name}. ex: {e.Message}");
                    Log.Debug(e.StackTrace);
                    return new DalResult(DalStatus.Unknown, "Error saving table");
                }
            });
        }

        public async Task<DalResult> UpdateRandomTable(RandomTable table)
        {
            return await Task.Run(() =>
            {
                try
                {
                    using var db = new DataStorage();
                    db.RandomTables.Update(table);
                    db.SaveChanges(true);

                    return new DalResult(DalStatus.Success);
                }
                catch (Exception e)
                {
                    Log.Error($"Exception updating table id {table.Id}. ex: {e.Message}");
                    Log.Debug(e.StackTrace);
                    return new DalResult(DalStatus.Unknown, "Error updating table");
                }
            });
        }

        public async Task<DalResult<RandomTable>> GetRandomTable(int id)
        {
            return await Task.Run(() =>
            {
                try
                {
                    using var db = new DataStorage();
                    var result = db.RandomTables
                        .Include(t => t.Entries)
                        .First(t => t.Id == id);
                    return new DalResult<RandomTable>(DalStatus.Success, result);

                }
                catch (Exception e) 
                {
                    Log.Error($"Exception getting table id {id}. ex: {e.Message}");
                    Log.Debug(e.StackTrace);
                    return new DalResult<RandomTable>(DalStatus.Unknown, null, "Unknown error retrieving table");
                }
            });
        }

        public async Task<DalResult<RandomTable>> GetRandomTable(string name)
        {
            return await Task.Run(() =>
            {
                try
                {
                    using var db = new DataStorage();
                    var result = db.RandomTables
                        .Include(t => t.Entries)
                        .First(t => t.Name == name);
                    return new DalResult<RandomTable>(DalStatus.Success, result);
                }
                catch (Exception e)
                {
                    Log.Error($"Exception getting table name {name}. ex: {e.Message}");
                    Log.Debug(e.StackTrace);
                    return new DalResult<RandomTable>(DalStatus.Unknown, null, "Unknown error retrieving table");
                }
            });
        }

        public async Task<DalResult<RandomTable[]>> GetAllRandomTables()
        {
            return await Task.Run(() =>
            {
                try
                {
                    using var db = new DataStorage();
                    var result = db.RandomTables.ToArray();
                    return new DalResult<RandomTable[]>(DalStatus.Success, result);
                }
                catch (Exception e)
                {
                    Log.Error($"Exception getting tables. ex: {e.Message}");
                    Log.Debug(e.StackTrace);
                    return new DalResult<RandomTable[]>(DalStatus.Unknown, null, "Unknown error retrieving tables");
                }
            });
        }

        public async Task<DalResult<List<string>>> GetCategories()
        {
            return await Task.Run(() =>
            {
                try
                {
                    using var db = new DataStorage();
                    var result = db.TableCategories.Select(x => x.Name).OrderBy(n => n).ToList();
                    return new DalResult<List<string>>(DalStatus.Success, result);
                }
                catch (Exception e)
                {
                    Log.Error($"Exception getting Table Categories. ex: {e.Message}");
                    Log.Debug(e.StackTrace);
                    return new DalResult<List<string>>(DalStatus.Unknown, null, "Unknown error retrieving categories");
                }
            });
        }

        public async Task<DalResult> AddCategory(string category)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    await using var db = new DataStorage();
                    var result = await db.TableCategories.AddAsync(new TableCategory() {Name = category});
                    db.SaveChanges(true);
                    return new DalResult(DalStatus.Success);
                }
                catch (Exception e)
                {
                    Log.Error($"Exception adding Table Category {category}. ex: {e.Message}");
                    Log.Debug(e.StackTrace);
                    return new DalResult(DalStatus.Unknown,"Error saving table");
                }
            });
        }

        public async Task<DalResult<bool>> CategoryExists(string name)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    await using var db = new DataStorage();
                    var exists = db.TableCategories.ToList().Any(x => x.Name == name);
                    return new DalResult<bool>(DalStatus.Success, exists);
                }
                catch (Exception e)
                {
                    Log.Error($"Exception checking if Table Category {name} exists. ex: {e.Message}");
                    Log.Debug(e.StackTrace);
                    return new DalResult<bool>(DalStatus.Unknown, false, "Error determining category exists");
                }
            });
        }
    }
}
