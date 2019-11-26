using System;
using System.Linq;
using System.Threading.Tasks;
using Mytheme.Dal;
using Mytheme.Dal.Dto;
using Mytheme.Services.Interfaces;
using Serilog;

namespace Mytheme.Services
{
    public class SectionService : ISectionService
    {
        private readonly DataStorage db;

        public SectionService(DataStorage db)
        {
            this.db = db;
        }


        public async Task<DalResult<Section[]>> GetAllCampaigns()
        {
            return await Task.Run( () =>
            {
                try
                {
                    var result = db.Sections.Where(x => x.SectionType == SectionType.Campaign).ToArray();
                    
                    return new DalResult<Section[]>(DalStatus.Success, result);
                }
                catch (Exception e)
                {
                    Log.Error($"Exception getting campaign list. ex: {e.Message}");
                    Log.Debug(e.StackTrace);
                    return new DalResult<Section[]>(DalStatus.Unknown, null, "Error saving section");
                }
            });
        }

        public async Task<DalResult> AddSection(Section section)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    var result = await db.Sections.AddAsync(section);
                    db.SaveChanges(true);

                    return new DalResult(DalStatus.Success);
                }
                catch (Exception e)
                {
                    Log.Error($"Exception saving section {section.Name}. ex: {e.Message}");
                    Log.Debug(e.StackTrace);
                    return new DalResult(DalStatus.Unknown, "Error saving section");
                }
            });
        }

        public async Task<DalResult> UpdateSection(Section section)
        {
            return await Task.Run(() =>
            {
                try
                {
                    db.Sections.Update(section);
                    db.SaveChanges(true);

                    return new DalResult(DalStatus.Success);
                }
                catch (Exception e)
                {
                    Log.Error($"Exception saving section {section.Name}. ex: {e.Message}");
                    Log.Debug(e.StackTrace);
                    return new DalResult(DalStatus.Unknown, "Error saving section");
                }
            });
        }

        public async Task<DalResult<Section>> GetSection(string id)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    var result = db.Sections
                        .First(x=> x.Id == id);

                    var childResult = await GetAllSectionsForParent(id);

                    result.Children = childResult.Result.ToList();

                    result.PageIds = db.Pages.Where(x => x.FK_Section == result.Id)
                        .Select(x => new PageLink(x.Name, x.Link)).ToList();

                    result.MapPageIds = db.MapPages.Where(x => x.FK_Section == result.Id)
                        .Select(x => new PageLink(x.Name, x.Link)).ToList();

                    return new DalResult<Section>( DalStatus.Success, result);
                }
                catch (Exception e)
                {
                    Log.Error($"Exception getting section id {id}. ex: {e.Message}");
                    Log.Debug(e.StackTrace);
                    return new DalResult<Section>(DalStatus.Unknown, null,"Error getting section");
                }
            });
        }

        public async Task<DalResult<Section[]>> GetAllSectionsForParent(string id)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    var result = db.Sections.Where(x => x.Enabled).Where(x => x.Parent == id).ToArray();

                    foreach (var section in result)
                    {
                        var childResult = await GetAllSectionsForParent(section.Id);

                        section.Children = childResult.Result.ToList();

                        section.PageIds = db.Pages.Where(x => x.FK_Section == section.Id)
                            .Select(x => new PageLink(x.Name, x.Link)).ToList();

                        section.MapPageIds = db.MapPages.Where(x => x.FK_Section == section.Id)
                            .Select(x => new PageLink(x.Name, x.Link)).ToList();
                    }

                    return new DalResult<Section[]>(DalStatus.Success, result);
                }
                catch (Exception e)
                {
                    Log.Error($"Exception getting all sections. ex: {e.Message}");
                    Log.Debug(e.StackTrace);
                    return new DalResult<Section[]>(DalStatus.Unknown, null, "Error getting sections");
                }
            });
        }
    }
}
