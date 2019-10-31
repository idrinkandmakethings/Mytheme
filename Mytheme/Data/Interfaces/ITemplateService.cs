using System.Collections.Generic;
using System.Threading.Tasks;
using Mytheme.Dal;
using Mytheme.Dal.Dto;

namespace Mytheme.Data.Interfaces
{
    public interface ITemplateService
    {
        Task<DalResult> AddTemplate(Template template);
        Task<DalResult> UpdateTemplate(Template template);
        Task<DalResult<Template>> GetTemplate(int id);
        Task<DalResult<Template>> GetTemplate(string name);
        Task<DalResult<Template[]>> GetAllTemplates();
        Task<DalResult<List<string>>> GetCategories();
        Task<DalResult> AddCategory(string category);

        Task<DalResult<bool>> CategoryExists(string name);
        Task<DalResult<bool>> TemplateExists(string name);
    }
}
