using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mytheme.Data;
using Mytheme.Data.Dto;

namespace Mytheme.Services.Interfaces
{
    public interface ITemplateService
    {
        Task<DalResult<Guid>> AddTemplate(Template template);
        Task<DalResult> UpdateTemplate(Template template);
        Task<DalResult<Template>> GetTemplate(Guid id);
        Task<DalResult<Template>> GetTemplateByName(string name);
        Task<DalResult<Template[]>> GetAllTemplates();
        Task<DalResult<List<string>>> GetCategories();
        Task<DalResult> AddCategory(string category);

        Task<DalResult<bool>> CategoryExists(string name);
        Task<DalResult<bool>> TemplateExists(string name);
    }
}
