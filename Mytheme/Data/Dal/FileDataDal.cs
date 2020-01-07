using System;
using System.Threading.Tasks;
using Dapper;
using Mytheme.Data.Dto;

namespace Mytheme.Data.Dal
{
    public class FileDataDal : BaseDal<FileData>
    {
        public FileDataDal(string connectionString) : base(connectionString)
        {
        }

        public override async Task<Guid> InsertAsync(FileData data)
        {
            await using var conn = GetConnection();

            try
            {
                await conn.OpenAsync();
                var id = await conn.InsertAsync(data);
                return data.Id;
            }
            finally
            {
                await conn.CloseAsync();
            } 
        }
    }
}
