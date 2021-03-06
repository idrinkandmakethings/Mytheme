﻿using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace Mytheme.Data.Dal
{
    /// <summary>
    /// https://github.com/ericdc1/Dapper.SimpleCRUD
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseIntIdDal<T>
    {
        protected readonly string connectionString;

        public BaseIntIdDal(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public virtual async Task<int> InsertAsync(T data)
        {
            await using var conn = GetConnection();

            try
            {
                await conn.OpenAsync();
                var id = await conn.InsertAsync<T>(data);
                return id.Value;
            }
            finally
            {
                await conn.CloseAsync();
            }
        }

        public virtual async Task<T> GetAsync(int id)
        {
            await using var conn = GetConnection();

            try
            {
                await conn.OpenAsync();
                var result = await conn.GetAsync<T>(id);
                return result;
            }
            finally
            {
                await conn.CloseAsync();
            }


        }

        public virtual async Task<T[]> GetAllAsync()
        {
            await using var conn = GetConnection();

            try
            {
                await conn.OpenAsync();
                var result = await conn.GetListAsync<T>();
                return result.ToArray();
            }
            finally
            {
                await conn.CloseAsync();
            }
        }

        public virtual async Task<bool> UpdateAsync(T data)
        {
            await using var conn = GetConnection();

            try
            {
                await conn.OpenAsync();
                var result = await conn.UpdateAsync(data);
                return result > 0;
            }
            finally
            {
                await conn.CloseAsync();
            }
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            await using var conn = GetConnection();

            try
            {
                await conn.OpenAsync();
                var result = await conn.DeleteAsync<T>(id);
                return result > 0;
            }
            finally
            {
                await conn.CloseAsync();
            }
        }


        protected SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(connectionString);
        }

    }
}
