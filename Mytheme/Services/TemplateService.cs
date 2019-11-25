using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mytheme.Dal;
using Mytheme.Dal.Dto;
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

        public async Task<DalResult> AddTemplate(Template template)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    template.SetVariables();
                    var result = await db.Templates.AddAsync(template);
                    db.SaveChanges(true);

                    return new DalResult(DalStatus.Success);
                }
                catch (Exception e)
                {
                    Log.Error($"Exception adding template. ex: {e.Message}");
                    Log.Debug(e.StackTrace);
                    return new DalResult(DalStatus.Unknown, "Error saving template");
                }
            });
        }

        public async Task<DalResult> UpdateTemplate(Template template)
        {
            return await Task.Run(() =>
            {
                try
                {
                    template.SaveVariables();
                    db.Templates.Update(template);
                    db.SaveChanges(true);

                    return new DalResult(DalStatus.Success);
                }
                catch (Exception e)
                {
                    Log.Error($"Exception updating template id {template.Id}. ex: {e.Message}");
                    Log.Debug(e.StackTrace);
                    return new DalResult(DalStatus.Unknown, "Error updating template");
                }
            });
        }

        public async Task<DalResult<Template>> GetTemplate(string id)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var result = db.Templates.First(t => t.Id == id);
                    result.SetVariables();
                    return new DalResult<Template>(DalStatus.Success, result);

                }
                catch (Exception e)
                {
                    Log.Error($"Exception getting template id {id}. ex: {e.Message}");
                    Log.Debug(e.StackTrace);
                    return new DalResult<Template>(DalStatus.Unknown, null, "Unknown error retrieving template");
                }
            });
        }

        public async Task<DalResult<Template>> GetTemplateByName(string name)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var result = db.Templates.First(t => t.Name == name);
                    result.SetVariables();
                    return new DalResult<Template>(DalStatus.Success, result);
                }
                catch (Exception e)
                {
                    Log.Error($"Exception adding template {name}. ex: {e.Message}");
                    Log.Debug(e.StackTrace);
                    return new DalResult<Template>(DalStatus.Unknown, null, "Unknown error retrieving template");
                }
            });
        }

        public async Task<DalResult<Template[]>> GetAllTemplates()
        {
            return await Task.Run(() =>
            {
                try
                {
                    var result = db.Templates.ToArray();
                    return new DalResult<Template[]>(DalStatus.Success, result);
                }
                catch (Exception e)
                {
                    Log.Error($"Exception getting all template. ex: {e.Message}");
                    Log.Debug(e.StackTrace);
                    return new DalResult<Template[]>(DalStatus.Unknown, null, "Unknown error retrieving templates");
                }
            });
        }

        public async Task<DalResult<List<string>>> GetCategories()
        {
            return await Task.Run(() =>
            {
                try
                {
                    var result = db.TemplateCategories.Select(x => x.Name).OrderBy(n => n).ToList();
                    return new DalResult<List<string>>(DalStatus.Success, result);
                }
                catch (Exception e)
                {
                    Log.Error($"Exception getting all template categories. ex: {e.Message}");
                    Log.Debug(e.StackTrace);
                    return new DalResult<List<string>>(DalStatus.Unknown, null, "Unknown error retrieving categories");
                }
            });
        }

        public async Task<DalResult> AddCategory(string category)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    var result = await db.TemplateCategories.AddAsync(new TemplateCategory { Name = category });
                    db.SaveChanges(true);
                    return new DalResult(DalStatus.Success);
                }
                catch (Exception e)
                {
                    Log.Error($"Exception adding template category {category}. ex: {e.Message}");
                    Log.Debug(e.StackTrace);
                    return new DalResult(DalStatus.Unknown, "Error saving category");
                }
            });
        }

        public async Task<DalResult<bool>> CategoryExists(string name)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var exists = db.TemplateCategories.Any(x => x.Name == name);
                    return new DalResult<bool>(DalStatus.Success, exists);
                }
                catch (Exception e)
                {
                    Log.Error($"Exception checking if template category {name} exists. ex: {e.Message}");
                    Log.Debug(e.StackTrace);
                    return new DalResult<bool>(DalStatus.Unknown, false, "Error determining category exists");
                }
            });
        }

        public async Task<DalResult<bool>> TemplateExists(string name)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var exists = db.Templates.Any(x => x.Name == name);
                    return new DalResult<bool>(DalStatus.Success, exists);
                }
                catch (Exception e)
                {
                    Log.Error($"Exception checking if template {name} exists. ex: {e.Message}");
                    Log.Debug(e.StackTrace);
                    return new DalResult<bool>(DalStatus.Unknown, false, "Error determining Template exists");
                }
            });
        }
    }
}