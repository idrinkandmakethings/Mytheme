using System.Collections.Generic;
using System.Threading.Tasks;
using Mytheme.Dal;
using Mytheme.Dal.Dto;

namespace Mytheme.Data.Interfaces
{
    public interface IRandomTableService
    {
        Task<DalResult> AddRandomTable(RandomTable table);
        Task<DalResult> UpdateRandomTable(RandomTable table);
        Task<DalResult<RandomTable>> GetRandomTable(string id);
        Task<DalResult<RandomTable>> GetRandomTableByName(string name);
        Task<DalResult<RandomTable[]>> GetAllRandomTables();
        Task<DalResult<List<string>>> GetCategories();
        Task<DalResult> AddCategory(string category);

        Task<DalResult<bool>> CategoryExists(string name);
        Task<DalResult<bool>> TableExists(string name);
    }
}