using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Mytheme.Data.Dto;

namespace Mytheme.Data.Dal
{
    public class SectionDal : BaseDal<Section>
    {
        public SectionDal(string connectionString) : base(connectionString)
        {
        }

        public async Task<Section[]> GetAllSubSectionsAsync(Guid id)
        {
            await using var conn = GetConnection();

            try
            {
                await conn.OpenAsync();
                var result = await conn.GetListAsync<Section>(new { Parent = id });
                return result.ToArray();
            }
            finally
            {
                await conn.CloseAsync();
            }
        }
    }
}
