using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Mytheme.Data.Dto;

namespace Mytheme.Data.Dal
{
    public class PageDal : BaseDal<Page>
    {
        public PageDal(string connectionString) : base(connectionString)
        {
        }

        public async Task<Page[]> GetAllForSectionAsync(Guid id)
        {
            await using var conn = GetConnection();

            try
            {
                await conn.OpenAsync();
                var result = await conn.GetListAsync<Page>(new { FK_Section = id });
                return result.ToArray();
            }
            finally
            {
                await conn.CloseAsync();
            }
        }

        public async Task<Page> GetByNameAsync(Guid parent, string name)
        {
            await using var conn = GetConnection();

            try
            {
                await conn.OpenAsync();
                var result = await conn.GetListAsync<Page>(new { FK_Section = parent, Name = name });
                return result.First();
            }
            finally
            {
                await conn.CloseAsync();
            }
        }
    }
}
