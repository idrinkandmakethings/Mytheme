using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mytheme.Dal;
using Mytheme.Dal.Dto;
using Mytheme.Data.Interfaces;
using Serilog;

namespace Mytheme.Data
{
    public class TemplateService : ITemplateService
    {
        public async Task<DalResult> AddTemplate(Template template)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    await using var db = new DataStorage();
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
                    using var db = new DataStorage();
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

        public async Task<DalResult<Template>> GetTemplate(int id)
        {
            return await Task.Run(() =>
            {
                try
                {
                    using var db = new DataStorage();
                    var result = db.Templates.First(t => t.Id == id);
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

        public async Task<DalResult<Template>> GetTemplate(string name)
        {
            return await Task.Run(() =>
            {
                try
                {
                    using var db = new DataStorage();
                    var result = db.Templates.First(t => t.Name == name);
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
                    using var db = new DataStorage();
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
                    using var db = new DataStorage();
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
                    await using var db = new DataStorage();
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
            return await Task.Run(async () =>
            {
                try
                {
                    await using var db = new DataStorage();
                    var exists = db.TemplateCategories.ToList().Any(x => x.Name == name);
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
            return await Task.Run(async () =>
            {
                try
                {
                    await using var db = new DataStorage();
                    var exists = db.Templates.ToList().Any(x => x.Name == name);
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