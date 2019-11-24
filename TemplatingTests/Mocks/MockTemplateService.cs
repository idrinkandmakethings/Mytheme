using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mytheme.Dal;
using Mytheme.Dal.Dto;
using Mytheme.Services.Interfaces;
using Mytheme.Templating;

namespace TemplatingTests.Mocks
{
    class MockTemplateService : ITemplateService
    {


        private Dictionary<string, Template> templates;

        public MockTemplateService()
        {

            templates = new Dictionary<string, Template>();
            SetUpTemplates();
        }


        public Task<DalResult> AddTemplate(Template template)
        {
            throw new System.NotImplementedException();
        }

        public Task<DalResult> UpdateTemplate(Template template)
        {
            throw new System.NotImplementedException();
        }

        public Task<DalResult<Template>> GetTemplate(string id)
        {
            throw new System.NotImplementedException();
        }

#pragma warning disable 1998
        public async Task<DalResult<Template>> GetTemplateByName(string name)
        {
            var template = templates[name];
            return new DalResult<Template>(DalStatus.Success, template);
        }
#pragma warning restore 1998

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
#pragma warning disable 1998
        public async Task<DalResult<bool>> TemplateExists(string name)
        {
            return new DalResult<bool>(DalStatus.Success, templates.ContainsKey(name));
        }
#pragma warning restore 1998

        private void SetUpTemplates()
        {
            var validator = new TemplateValidator(new MockRandomTableService(), this);

            var testBody = @"2d4+10=[die:2d4+10]
-6to6=[rng:-6:6]
list=[lst:thing one, thing two, thing three]
table=[tbl:Test Table]
";

            var testTemplate = new Template
            {
                Id = Guid.Empty.ToString(),
                Name = "Test Template",
                Category = "Test",
                Description = "Test",
                Enabled = true,
                TemplateBody = testBody
            };

            var result = validator.ValidateTemplate(testTemplate).Result;

            templates["Test Template"] = result.Template;
        }
    }
}
