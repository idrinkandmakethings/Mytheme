using System.Collections.Generic;
using System.Threading.Tasks;
using Mytheme.Dal;
using Mytheme.Dal.Dto;
using Mytheme.Data.Interfaces;

namespace TemplatingTests.Mocks
{
    class MockTemplateService : ITemplateService
    {
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

        public Task<DalResult<bool>> TemplateExists(string name)
        {
            throw new System.NotImplementedException();
        }
    }
}
