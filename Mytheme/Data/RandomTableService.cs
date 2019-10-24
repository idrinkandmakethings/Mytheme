using System.Linq;
using System.Threading.Tasks;
using Mytheme.Dal;
using Mytheme.Dal.Dto;

namespace Mytheme.Data
{
    public class RandomTableService
    {
        public async Task<RandomTable> AddRandomTable(RandomTable table)
        {
            return await Task.Run(async () =>
            {
                using var db = new DataStorage();

                var result = await db.RandomTables.AddAsync(table);
                db.SaveChanges(true);
                return result.Entity;
            });
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
                return db.RandomTables.Single(t => t.TableId == id);
            });
        }

        public async Task<RandomTable> GetRandomTable(string name)
        {
            return await Task.Run(() => { 
                using var db = new DataStorage();
                return db.RandomTables.Single(t => t.TableName == name);
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
    }
}
