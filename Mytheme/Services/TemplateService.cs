using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mytheme.Data;
using Mytheme.Data.Dto;
using Mytheme.Services.Interfaces;
using Serilog;

namespace Mytheme.Services
{
    public class TemplateService : ITemplateService
    {

        private readonly DataStorage db;

        public TemplateService(DataStorage db)
        {
            this.db = db;
        }

        public async Task<DalResult<Guid>> AddTemplate(Template template)
        {
            try
            {
                var id = await db.Template.InsertAsync(template);

                if (template.Fields.Count > 0)
                {
                    foreach (var field in template.Fields)
                    {
                        field.FK_Template = id;
                        field.Id = await db.TemplateField.InsertAsync(field);
                    }
                }

                return new DalResult<Guid>(DalStatus.Success, id);
            }
            catch (Exception e)
            {
                Log.Error(e, "Exception saving table {Name}.", template.Name);
                return new DalResult<Guid>(DalStatus.Unknown, Guid.Empty, "Error saving table");
            }
        }

        public async Task<DalResult> UpdateTemplate(Template template)
        {
            try
            {
                var result = await db.Template.UpdateAsync(template);

                await db.TemplateField.DeleteAllForTemplateAsync(template.Id);

                if (template.Fields.Count > 0)
                {
                    foreach (var entry in template.Fields)
                    {
                        if (entry.Id == Guid.Empty)
                        {
                            entry.FK_Template = template.Id;
                            entry.Id = await db.TemplateField.InsertAsync(entry);
                        }
                        else
                        {
                            await db.TemplateField.UpdateAsync(entry);
                        }
                    }
                }

                return new DalResult(result ? DalStatus.Success : DalStatus.Unknown);
            }
            catch (Exception e)
            {
                Log.Error(e, "Exception updating template id {Id}.", template);
                return new DalResult(DalStatus.Unknown, "Error updating template");
            }
        }

        public async Task<DalResult<Template>> GetTemplate(Guid id)
        {
            try
            {
                var result = await db.Template.GetAsync(id);
                result.Fields = await db.TemplateField.GetByTemplateIdAsync(result.Id);
                return new DalResult<Template>(DalStatus.Success, result);
            }
            catch (Exception e)
            {
                Log.Error(e, "Exception getting template id {Id}.", id);
                return new DalResult<Template>(DalStatus.Unknown, null, "Unknown error retrieving template");
            }
        }

        public async Task<DalResult<Template>> GetTemplateByName(string name)
        {
            try
            {
                var result = await db.Template.GetByNameAsync(name);
                result.Fields = await db.TemplateField.GetByTemplateIdAsync(result.Id);
                return new DalResult<Template>(DalStatus.Success, result);
            }
            catch (Exception e)
            {
                Log.Error(e, "Exception getting template name {Name}.", name);
                return new DalResult<Template>(DalStatus.Unknown, null, "Unknown error retrieving template");
            }
        }

        public async Task<DalResult<Template[]>> GetAllTemplates()
        {
            try
            {
                var result = await db.Template.GetAllAsync();
                return new DalResult<Template[]>(DalStatus.Success, result);
            }
            catch (Exception e)
            {
                Log.Error(e, "Exception getting tables.");
                return new DalResult<Template[]>(DalStatus.Unknown, null, "Unknown error retrieving templates");
            }
        }

        public async Task<DalResult<List<string>>> GetCategories()
        {
            try
            {
                var result = await db.TemplateCategory.GetAllAsync();
                return new DalResult<List<string>>(DalStatus.Success, result.Select(x => x.Name).ToList());
            }
            catch (Exception e)
            {
                Log.Error(e, $"Exception getting Template Categories.");
                return new DalResult<List<string>>(DalStatus.Unknown, null, "Unknown error retrieving categories");
            }
        }

        public async Task<DalResult> AddCategory(string category)
        {
            try
            {
                _ = await db.TemplateCategory.InsertAsync(new TemplateCategory() { Name = category, Enabled = true });
                return new DalResult(DalStatus.Success);
            }
            catch (Exception e)
            {
                Log.Error(e, "Exception adding Template Category {Category}", category);
                return new DalResult(DalStatus.Unknown, "Error saving template category");
            }
        }

        public async Task<DalResult<bool>> CategoryExists(string name)
        {
            try
            {
                var exists = await db.TemplateCategory.Exists(name);
                return new DalResult<bool>(DalStatus.Success, exists);
            }
            catch (Exception e)
            {
                Log.Error(e, "Exception checking if Template Category {Name} exists.", name);
                return new DalResult<bool>(DalStatus.Unknown, false, "Error determining category exists");
            }
        }

        public async Task<DalResult<bool>> TemplateExists(string name)
        {
            try
            {
                var exists = await db.Template.Exists(name);
                return new DalResult<bool>(DalStatus.Success, exists);
            }
            catch (Exception e)
            {
                Log.Error(e, "Exception checking if template {name} exists", name);
                return new DalResult<bool>(DalStatus.Unknown, false, "Error determining template exists");
            }
        }
    }
}