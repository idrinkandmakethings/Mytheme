using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mytheme.Data;
using Mytheme.Data.Dto;

namespace Mytheme.Services.Interfaces
{
    public interface IRandomTableService
    {
        Task<DalResult<Guid>> AddRandomTable(RandomTable table);
        Task<DalResult> UpdateRandomTable(RandomTable table);
        Task<DalResult<RandomTable>> GetRandomTable(Guid id);
        Task<DalResult<RandomTable>> GetRandomTableByName(string name);
        Task<DalResult<RandomTable[]>> GetAllRandomTables();
        Task<DalResult<List<string>>> GetCategories();
        Task<DalResult> AddCategory(string category);

        Task<DalResult<bool>> CategoryExists(string name);
        Task<DalResult<bool>> TableExists(string name);
    }
}