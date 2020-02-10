using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Mytheme.Data.Dto;

namespace Mytheme.Data.Dal
{
    public class TableCategoryDal : BaseDal<TableCategory>
    {
        public TableCategoryDal(string connectionString) : base(connectionString)
        {
        }

        public async Task<bool> Exists(string name)
        {
            await using var conn = GetConnection();

            try
            {
                await conn.OpenAsync();
                var result = await conn.RecordCountAsync<TableCategory>(new { Name = name });
                return result < 0;
            }
            finally
            {
                await conn.CloseAsync();
            }
        }
    }

    public class RandomTableDal : BaseDal<RandomTable>
    {
        public RandomTableDal(string connectionString) : base(connectionString)
        {
        }

        public async Task<RandomTable> GetByNameAsync(string name)
        {
            await using var conn = GetConnection();

            try
            {
                await conn.OpenAsync();
                var result = await conn.GetListAsync<RandomTable>(new {Name = name});
                return result.FirstOrDefault();
            }
            finally
            {
                await conn.CloseAsync();
            }
        }

        public async Task<bool> Exists(string name)
        {
            await using var conn = GetConnection();

            try
            {
                await conn.OpenAsync();
                var result = await conn.RecordCountAsync<RandomTable>(new { Name = name });
                return result > 0;
            }
            finally
            {
                await conn.CloseAsync();
            }
        }
    }

    public class TableEntryDal : BaseDal<TableEntry>
    {
        public TableEntryDal(string connectionString) : base(connectionString)
        {
        }

        public async Task<List<TableEntry>> GetByTableIdAsync(Guid id)
        {
            await using var conn = GetConnection();

            try
            {
                await conn.OpenAsync();
                var result = await conn.GetListAsync<TableEntry>(new { FK_RandomTable = id});
                return result.ToList();
            }
            finally
            {
                await conn.CloseAsync();
            }
        }

        public async Task DeleteAllForTableAsync(Guid id)
        {
            await using var conn = GetConnection();

            try
            {
                await conn.OpenAsync();
                await conn.DeleteListAsync<TableEntry>(new { FK_RandomTable = id });
            }
            finally
            {
                await conn.CloseAsync();
            }
        }
    }
}
