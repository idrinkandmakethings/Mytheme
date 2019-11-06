using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mytheme.Dal;
using Mytheme.Dal.Dto;
using Mytheme.Data.Interfaces;

namespace TemplatingTests.Mocks
{
    class MockRandomTableService : IRandomTableService
    {
        
        private Dictionary<string, RandomTable> tables;

        public MockRandomTableService()
        {
            tables = new Dictionary<string, RandomTable>();
            SetUpTables();
        }


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

        public async Task<DalResult<RandomTable>> GetRandomTable(string name)
        {
            var result = tables[name];
            return new DalResult<RandomTable>(DalStatus.Success, result);
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
            return new DalResult<bool>(DalStatus.Success, tables.ContainsKey(name));
        }

        private void SetUpTables()
        {
            var testTable = new RandomTable
            {
                Id = 1,
                Name = "Test Table",
                Category = "Test",
                Description = "Test",
                Enabled = true,
                Entries = new List<TableEntry>
                {
                    new TableEntry
                    {
                        Id = 1,
                        RandomTableForeignKey = 1,
                        Entry = "Table entry 1",
                        UpperBound = 1,
                        LowerBound = 1,
                    },
                    new TableEntry
                    {
                        Id = 2,
                        RandomTableForeignKey = 1,
                        Entry = "Table entry 2",
                        UpperBound = 5,
                        LowerBound = 2,
                    },
                    new TableEntry
                    {
                        Id = 3,
                        RandomTableForeignKey = 1,
                        Entry = "Table entry 3",
                        UpperBound = 6,
                        LowerBound = 6,
                    },
                    new TableEntry
                    {
                        Id = 4,
                        RandomTableForeignKey = 1,
                        Entry = "Table entry 4",
                        UpperBound = 7,
                        LowerBound = 7,
                    },
                }
            };

            tables["Test Table"] = testTable;

            var raceTable = new RandomTable
            {
                Id = 1,
                Name = "race",
                Category = "Test",
                Description = "Test",
                Enabled = true,
                Entries = new List<TableEntry>()
            };

            tables["race"] = raceTable;


            var lastNamesTable = new RandomTable
            {
                Id = 1,
                Name = "Last Names",
                Category = "Test",
                Description = "Test",
                Enabled = true,
                Entries = new List<TableEntry>()
            };

            tables["Last Names"] = raceTable;
        }
    }
}
