using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Mytheme.Data.Dto;

public class SettingDal
{
    private readonly string connectionString;

    public SettingDal(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public async Task<bool> InsertAsync(Setting data)
    {
        await using var conn = GetConnection();

        try
        {
            await conn.OpenAsync();
            await conn.InsertAsync(data);
            return true;
        }
        finally
        {
            await conn.CloseAsync();
        }
    }

    public async Task<Setting> GetAsync(string id)
    {
        await using var conn = GetConnection();

        try
        {
            await conn.OpenAsync();
            var result = await conn.GetAsync<Setting>(id);
            return result;
        }
        finally
        {
            await conn.CloseAsync();
        }
    }

    public async Task<Setting[]> GetAllAsync()
    {
        await using var conn = GetConnection();

        try
        {
            await conn.OpenAsync();
            var result = await conn.GetListAsync<Setting>();
            return result.ToArray();
        }
        finally
        {
            await conn.CloseAsync();
        }
    }

    public async Task<bool> UpdateSetting(Setting data)
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

    public async Task<bool> DeleteAsync(string id)
    {
        await using var conn = GetConnection();

        try
        {
            await conn.OpenAsync();
            var result = await conn.DeleteAsync<Setting>(id);
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