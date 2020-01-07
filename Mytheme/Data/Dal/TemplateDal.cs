using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Mytheme.Data.Dto;

namespace Mytheme.Data.Dal
{
    public class TemplateCategoryDal : BaseDal<TemplateCategory>
    {
        public TemplateCategoryDal(string connectionString) : base(connectionString)
        {
        }

        public async Task<bool> Exists(string name)
        {
            await using var conn = GetConnection();

            try
            {
                await conn.OpenAsync();
                var result = await conn.RecordCountAsync<TemplateCategory>(new { Name = name });
                return result < 0;
            }
            finally
            {
                await conn.CloseAsync();
            }
        }
    }

    public class TemplateDal : BaseDal<Template>
    {
        public TemplateDal(string connectionString) : base(connectionString)
        {
        }

        public async Task<Template> GetByNameAsync(string name)
        {
            await using var conn = GetConnection();

            try
            {
                await conn.OpenAsync();
                var result = await conn.GetListAsync<Template>(new { Name = name });
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
                var result = await conn.RecordCountAsync<Template>(new { Name = name });
                return result < 0;
            }
            finally
            {
                await conn.CloseAsync();
            }
        }
    }

    public class TemplateFieldDal : BaseDal<TemplateField>
    {
        public TemplateFieldDal(string connectionString) : base(connectionString)
        {
        }

        public async Task DeleteAllForTemplateAsync(Guid id)
        {
            await using var conn = GetConnection();

            try
            {
                await conn.OpenAsync();
                await conn.DeleteListAsync<TemplateField>(new {FK_Template = id});
            }
            finally
            {
                await conn.CloseAsync();
            }

        }

        public async Task<List<TemplateField>> GetByTemplateIdAsync(Guid id)
        {
            await using var conn = GetConnection();

            try
            {
                await conn.OpenAsync();
                var result = await conn.GetListAsync<TemplateField>(new { FK_Template = id });
                return result.ToList();
            }
            finally
            {
                await conn.CloseAsync();
            }
        }
    }
}

