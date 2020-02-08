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
    public class TagDal : BaseIntIdDal<Tag>
    {
        public TagDal(string connectionString) : base(connectionString)
        {
        }

        public async Task<List<Tag>> GetTagsForPage(Guid id)
        {
            var sql = $@"SELECT * FROM {Tables.Tag}
                         INNER JOIN {Tables.TagMap} AS m
                         WHERE m.PageId = @PageId";

            await using var conn = GetConnection();
            
            var tags = await conn.QueryAsync<Tag>(sql, new { PageId = id.ToString() });
            return tags.ToList();
        }

        public async Task AddTagForPage(Guid page, int tag)
        {
            var sql = $@"SELECT COUNT(*) FROM {Tables.TagMap}
                         WHERE TagId = @TagId AND PageId = @PageId";

            await using var conn = GetConnection();

            var count = await conn.ExecuteScalarAsync<int>(sql, new {TagId = tag, PageId = page.ToString()});

            if (count > 0)
            {
                return;
            }

            sql = $@"INSERT INTO {Tables.TagMap} (TagId, PageId)
                     VALUES(@TagId, @PageId)";

            await conn.ExecuteAsync(sql, new {TagId = tag, PageId = page.ToString()});
        }
        
        public async Task DeleteTagForPage(Guid page, int tag)
        {
            var sql = $@"DELETE FROM {Tables.TagMap}
                         WHERE TagId = @TagId AND PageId = @PageId";

            await using var conn = GetConnection();

            await conn.ExecuteAsync(sql, new { TagId = tag, PageId = page.ToString() });
        }

        public async Task<List<NavigationLink>> GetPageLinksForTag(int id)
        {
            var result = new List<NavigationLink>();

            var sql = $@"SELECT * FROM {Tables.Page}
                         INNER JOIN {Tables.TagMap} AS m
                         WHERE m.TagId = @TagId";

            await using var conn = GetConnection();

            var pages = await conn.QueryAsync<Page>(sql, new { TagId = id });
            
            result.AddRange(pages.Select(x => new NavigationLink(x.Id, ViewType.Page, x.Name)));

            return result;
        }

        public async Task<List<NavigationLink>> GetMapLinksForTag(int id)
        {
            var result = new List<NavigationLink>();

            var sql = $@"SELECT * FROM {Tables.MapPage}
                         INNER JOIN {Tables.TagMap} AS m
                         WHERE m.TagId = @TagId";

            await using var conn = GetConnection();

            var maps = await conn.QueryAsync<MapPage>(sql, new { TagId = id });

            result.AddRange(maps.Select(x => new NavigationLink(x.Id, ViewType.MapPage, x.Name)));

            return result;
        }
    }
}
