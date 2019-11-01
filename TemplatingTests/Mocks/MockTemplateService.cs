using System.Collections.Generic;
using System.Threading.Tasks;
using Mytheme.Dal;
using Mytheme.Dal.Dto;
using Mytheme.Data.Interfaces;

namespace TemplatingTests.Mocks
{
    class MockTemplateService : ITemplateService
    {
        private readonly List<string> templates = new List<string> { "Test Template" };

        public Task<DalResult> AddTemplate(Template template)
        {
            throw new System.NotImplementedException();
        }

        public Task<DalResult> UpdateTemplate(Template template)
        {
            throw new System.NotImplementedException();
        }

        public Task<DalResult<Template>> GetTemplate(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<DalResult<Template>> GetTemplate(string name)
        {
            throw new System.NotImplementedException();
        }

        public Task<DalResult<Template[]>> GetAllTemplates()
        {
            throw new System.NotImplementedException();
        }

        public Task<DalResult<List<string>>> GetCategories()
        {
            throw new System.NotImplementedException();
        }

        public Task<DalResult> AddCategory(string category)
        {
            throw new System.NotImplementedException();
        }

        public Task<DalResult<bool>> CategoryExists(string name)
        {
            throw new System.NotImplementedException();
        }

        public async Task<DalResult<bool>> TemplateExists(string name)
        {
            return new DalResult<bool>(DalStatus.Success, templates.Contains(name));
        }
    }
}
