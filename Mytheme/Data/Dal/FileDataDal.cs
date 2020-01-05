using System;
using System.Threading.Tasks;
using DapperExtensions;
using Mytheme.Data.Dto;

namespace Mytheme.Data.Dal
{
    public class FileDataDal : BaseDal
    {
        public FileDataDal(string connectionString) : base(connectionString)
        {
        }

        public async Task Insert(FileData data)
        {
            await using var conn = GetConnection();
        
            try
            {
                await conn.OpenAsync();
                await conn.Insert(data);
            }
            finally
            {
                await conn.CloseAsync();
            }
            
        }

        public async Task<FileData> Get(Guid id)
        {
            await using var conn = GetConnection();

            try
            {
                await conn.OpenAsync();
                var result = conn.Get<FileData>(id);
                return result;
            }
            finally
            {
                await conn.CloseAsync();
            }

        }
    }
}
