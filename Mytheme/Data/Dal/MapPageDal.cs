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
    public class MapPageDal : BaseDal<MapPage>
    {
        public MapPageDal(string connectionString) : base(connectionString)
        {
        }

        public async Task<MapPage[]> GetAllForSectionAsync(Guid id)
        {
            await using var conn = GetConnection();

            try
            {
                await conn.OpenAsync();
                var result = await conn.GetListAsync<MapPage>(new { FK_Section = id });
                return result.ToArray();
            }
            finally
            {
                await conn.CloseAsync();
            }
        }

        public async Task<List<LinkObject>> GetAllLinkObjectsForSectionAsync(Guid id)
        {
            var sql = $@"SELECT Name, Id as Link FROM {Tables.MapPage}
                         WHERE FK_Section = @fk_id";
            await using var conn = GetConnection();

            try
            {
                var tags = await conn.QueryAsync<NavigationLink>(sql, new { fk_id = id.ToString() });

                return tags.Select(x =>
                {
                    x.ViewType = ViewType.MapPage;
                    return new LinkObject(x.Name, x);
                }).ToList();
            }
            finally
            {
                await conn.CloseAsync();
            }
        }
    }

    public class MapMarkerDal : BaseDal<MapMarker>
    {
        public MapMarkerDal(string connectionString) : base(connectionString)
        {
        }

        public async Task<MapMarker[]> GetAllForMapAsync(Guid id)
        {
            await using var conn = GetConnection();

            try
            {
                await conn.OpenAsync();
                var result = await conn.GetListAsync<MapMarker>(new { FK_MapPage = id });
                return result.ToArray();
            }
            finally
            {
                await conn.CloseAsync();
            }
        }
    }
}
