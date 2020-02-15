using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Mytheme.Data.Dto;
using Mytheme.Data.SQL;
using Mytheme.Models;

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

        public async Task<List<LinkObject>> GetAllLinkObjectsForSectionAsync(Guid id)
        {
            var sql = $@"SELECT Name, Id as Link FROM {Tables.Page}
                         WHERE FK_Section = @fk_id";
            await using var conn = GetConnection();

            try
            {
                var tags = await conn.QueryAsync<NavigationLink>(sql, new {fk_id = id.ToString()});

                return tags.Select(x =>
                { 
                    x.ViewType = ViewType.Page;
                    return new LinkObject(x.Name, x);
                }).ToList();
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
