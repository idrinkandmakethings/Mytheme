using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mytheme.Dal;
using Mytheme.Dal.Dto;

namespace Mytheme.Data
{
    public class RandomTableService
    {
        public async Task<(bool success, string result)> AddRandomTable(RandomTable table)
        {
            var success = false;
            var result = string.Empty;
            
            await Task.Run(async () =>
            {
                try
                {
                    await using var db = new DataStorage();
                    var result = await db.RandomTables.AddAsync(table);
                    db.SaveChanges(true);
                    if (result.Entity.Id > 0)
                    {
                        success = true;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    result = "Error saving table";
                }
            });

            return (success, result);
        }

        public async Task<int> UpdateRandomTable(RandomTable table)
        {
            return await Task.Run(() =>
            {

                using var db = new DataStorage();

                db.RandomTables.Update(table);
                return db.SaveChanges(true);

            });
        }

        public async Task<RandomTable> GetRandomTable(int id)
        {
            return await Task.Run(() => {
                using var db = new DataStorage();
                return db.RandomTables.Single(t => t.Id == id);
            });
        }

        public async Task<RandomTable> GetRandomTable(string name)
        {
            return await Task.Run(() => { 
                using var db = new DataStorage();
                return db.RandomTables.Single(t => t.Name == name);
            });
        }

        public async Task<RandomTable[]> GetAllRandomTables()
        {
            return await Task.Run(() =>
            {
                using var db = new DataStorage();
                return db.RandomTables.ToArray();
            });
        }

        public async Task<List<string>> GetCategories()
        {
            return await Task.Run(() =>
            {
                using var db = new DataStorage();
                var result = db.TableCategories.Select(x => x.Name).OrderBy(n => n).ToList();
                return result;
            });
        }

        public async Task<(bool success, string result)> AddCategory(string category)
        {
            var success = false;
            var result = string.Empty;

            await Task.Run(async () =>
            {
                try
                {
                    await using var db = new DataStorage();
                    var result = await db.TableCategories.AddAsync(new TableCategory(){Name = category});
                    db.SaveChanges(true);
                    if (result.Entity.Id > 0)
                    {
                        success = true;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    result = "Error saving table";
                }
            });

            return (success, result);
        }
    }
}
