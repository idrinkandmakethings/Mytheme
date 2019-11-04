using System.Collections.Generic;
using System.Threading.Tasks;
using Mytheme.Dal;
using Mytheme.Dal.Dto;
using Mytheme.Data.Interfaces;

namespace TemplatingTests.Mocks
{
    class MockRandomTableService : IRandomTableService
    {
        private readonly List<string> tables = new List<string>{"Test Table","race", "Last Names"};

        public Task<DalResult> AddRandomTable(RandomTable table)
        {
            throw new System.NotImplementedException();
        }

        public Task<DalResult> UpdateRandomTable(RandomTable table)
        {
            throw new System.NotImplementedException();
        }

        public Task<DalResult<RandomTable>> GetRandomTable(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<DalResult<RandomTable>> GetRandomTable(string name)
        {
            throw new System.NotImplementedException();
        }

        public Task<DalResult<RandomTable[]>> GetAllRandomTables()
        {
            throw new System.NotImplementedException();
        }

        public Task<DalResult<List<string>>> GetCategories()
        {
            throw new System.NotImplementedException();
        }

        public Task<DalResult> AddCategory(string category)
        {
            throw new System.NotImplementedException();
        }

        public Task<DalResult<bool>> CategoryExists(string name)
        {
            throw new System.NotImplementedException();
        }

        public async Task<DalResult<bool>> TableExists(string name)
        {
            return new DalResult<bool>(DalStatus.Success, tables.Contains(name));
        }
    }
}
