using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mytheme.Data;
using Mytheme.Data.Dto;
using Mytheme.Services.Interfaces;

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


        public Task<DalResult<Guid>> AddRandomTable(RandomTable table)
        {
            throw new System.NotImplementedException();
        }

        public Task<DalResult> UpdateRandomTable(RandomTable table)
        {
            throw new System.NotImplementedException();
        }

        public Task<DalResult<RandomTable>> GetRandomTable(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<DalResult<RandomTable>> GetRandomTable(string id)
        {
           throw new System.NotImplementedException();
        }

#pragma warning disable 1998
        public async Task<DalResult<RandomTable>> GetRandomTableByName(string name)
        {
            var result = tables[name];
            return new DalResult<RandomTable>(DalStatus.Success, result);
        }
#pragma warning restore 1998

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

#pragma warning disable 1998
        public async Task<DalResult<bool>> TableExists(string name)
        {
            return new DalResult<bool>(DalStatus.Success, tables.ContainsKey(name));
        }
#pragma warning restore 1998

        private void SetUpTables()
        {
            var testTable = new RandomTable
            {
                Id = Guid.Empty,
                Name = "Test Table",
                Category = "Test",
                Description = "Test",
                Enabled = true,
                Entries = new List<TableEntry>
                {
                    new TableEntry
                    {
                        Id = Guid.NewGuid(),
                        FK_RandomTable = Guid.Empty,
                        Entry = "Table entry 1",
                        UpperBound = 1,
                        LowerBound = 1,
                    },
                    new TableEntry
                    {
                        Id = Guid.NewGuid(),
                        FK_RandomTable = Guid.Empty,
                        Entry = "Table entry 2",
                        UpperBound = 5,
                        LowerBound = 2,
                    },
                    new TableEntry
                    {
                        Id = Guid.NewGuid(),
                        FK_RandomTable = Guid.Empty,
                        Entry = "Table entry 3",
                        UpperBound = 6,
                        LowerBound = 6,
                    },
                    new TableEntry
                    {
                        Id = Guid.NewGuid(),
                        FK_RandomTable = Guid.Empty,
                        Entry = "Table entry 4",
                        UpperBound = 7,
                        LowerBound = 7,
                    },
                }
            };

            tables["Test Table"] = testTable;

            var raceTable = new RandomTable
            {
                Id = Guid.Empty,
                Name = "race",
                Category = "Test",
                Description = "Test",
                Enabled = true,
                Entries = new List<TableEntry>()
            };

            tables["race"] = raceTable;


            var lastNamesTable = new RandomTable
            {
                Id = Guid.Empty,
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
