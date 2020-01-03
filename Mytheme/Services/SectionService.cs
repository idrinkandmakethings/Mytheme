using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mytheme.Dal;
using Mytheme.Dal.Dto;
using Mytheme.Models;
using Mytheme.Services.Interfaces;
using Mytheme.Utility;
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


        public async Task<DalResult<Section[]>> GetAllCampaignsAsync()
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

        public async Task<DalResult<IndexLevel>> GetCampaignIndex(string id)
        {
            return await Task.Run(async () =>
            {
                var result = await GetIndexLevelForSection(id);
                return new DalResult<IndexLevel>(DalStatus.Success, result);
            }).ConfigureAwait(false);
        }

        private async Task<IndexLevel> GetIndexLevelForSection(string id)
        {
            try
            {
                var section = db.Sections.First(x => x.Id == id);

                var result = new IndexLevel
                {
                    LevelHome = new LinkObject {Link = $"section/{section.Id}", Name = section.Name},
                    Type = section.SectionType,
                    SubSectionType = section.SectionType.GetSubSectionType()
                };
                
                result.Maps = db.MapPages.Where(x => x.FK_Section == id)
                    .Select(x => new LinkObject {Link = x.Link, Name = x.Name}).ToList();

                result.Pages = db.Pages.Where(x => x.FK_Section == id)
                    .Select(x => new LinkObject {Link = x.Link, Name = x.Name}).ToList();

                result.SubLevels = new List<IndexLevel>();

                if (result.SubSectionType == SectionType.None)
                {
                    return result;
                }
                else
                {
                    var subs = db.Sections.Where(x => x.Enabled).Where(x => x.Parent == id).ToArray();

                    foreach (var sub in subs)
                    {
                        result.SubLevels.Add(await GetIndexLevelForSection(sub.Id));
                    }
                }

                return result;
            }
            catch (Exception e)
            {
                Log.Error(e, "Exception getting index level for id {Id}.", id);
                return null;
            }
        }

        public async Task<DalResult> AddSectionAsync(Section section)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    section.Id = Guid.NewGuid().ToString();
                    section.DateCreated = DateTime.Now;
                    section.DateModified = DateTime.Now;
                    
                   if (!section.TryValidate(out var errors))
                   {
                       section.Id = Guid.Empty.ToString();
                       return new DalResult(DalStatus.ConstraintViolation, string.Join(';', errors));
                   }
                    
                    var result = await db.Sections.AddAsync(section);
                    db.SaveChanges(true);

                    return new DalResult(DalStatus.Success, section.Id);
                }
                catch (Exception e)
                {
                    Log.Error($"Exception saving section {section.Name}. ex: {e.Message}");
                    Log.Debug(e.StackTrace);
                    return new DalResult(DalStatus.Unknown, "Error saving section");
                }
            });
        }

        public async Task<DalResult> UpdateSectionAsync(Section section)
        {
            return await Task.Run(() =>
            {
                try
                {
                    section.DateModified = DateTime.Now;

                    if (!section.TryValidate(out var errors))
                    {
                        return new DalResult(DalStatus.ConstraintViolation, string.Join(';', errors));
                    }

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

        public async Task<DalResult<Section>> GetSectionAsync(string id)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    var result = db.Sections.Find(id);


                    var childResult = await GetAllSectionsForParentAsync(id);

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

        public async Task<DalResult<Section[]>> GetAllSectionsForParentAsync(string id)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    var result = db.Sections.Where(x => x.Enabled).Where(x => x.Parent == id).ToArray();

                    foreach (var section in result)
                    {
                        var childResult = await GetAllSectionsForParentAsync(section.Id);

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

        #region Page Queries


        public async Task<DalResult<Page[]>> GetAllPagesForSection(string id)
        {
            return await Task.Run( () =>
            {
                try
                {
                    var result = db.Pages.Where(x => x.FK_Section == id).ToArray();

                    return new DalResult<Page[]>(DalStatus.Success, result);
                }
                catch (Exception e)
                {
                    Log.Error($"Exception getting pages for section id {id}. ex: {e.Message}");
                    Log.Debug(e.StackTrace);
                    return new DalResult<Page[]>(DalStatus.Unknown, null, "Error getting pages for section");
                }
            });
        }

        public async Task<DalResult> AddPageAsync(Page page)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    page.Id = Guid.NewGuid().ToString();
                    page.DateCreated = DateTime.Now;
                    page.DateModified = DateTime.Now;

                    if (!page.TryValidate(out var errors))
                    {
                        page.Id = Guid.Empty.ToString();
                        return new DalResult(DalStatus.ConstraintViolation, string.Join(';', errors));
                    }

                    var result = await db.Pages.AddAsync(page);
                    db.SaveChanges(true);

                    return new DalResult(DalStatus.Success, page.Id);
                }
                catch (Exception e)
                {
                    Log.Error($"Exception saving page {page.Name}. ex: {e.Message}");
                    Log.Debug(e.StackTrace);
                    return new DalResult(DalStatus.Unknown, "Error saving page");
                }
            });
        }

        public async Task<DalResult> UpdatePageAsync(Page page)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    page.DateModified = DateTime.Now;

                    if (!page.TryValidate(out var errors))
                    {
                        page.Id = Guid.Empty.ToString();
                        return new DalResult(DalStatus.ConstraintViolation, string.Join(';', errors));
                    }

                    var result = await db.Pages.AddAsync(page);
                    db.SaveChanges(true);

                    return new DalResult(DalStatus.Success, page.Id);
                }
                catch (Exception e)
                {
                    Log.Error($"Exception saving page {page.Name}. ex: {e.Message}");
                    Log.Debug(e.StackTrace);
                    return new DalResult(DalStatus.Unknown, "Error saving page");
                }
            });
        }

        public async Task<DalResult<Page>> GetPageAsync(string id)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    var result = await db.Pages.FindAsync(id);

                    return new DalResult<Page>(DalStatus.Success, result);
                }
                catch (Exception e)
                {
                    Log.Error($"Exception getting page id {id}. ex: {e.Message}");
                    Log.Debug(e.StackTrace);
                    return new DalResult<Page>(DalStatus.Unknown, null, "Error getting page");
                }
            });
        }

        public async Task<DalResult<Page>> GetPageByNameAsync(string parent, string name)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    var result = db.Pages.First(x => x.Enabled && x.Name == name && x.FK_Section == parent);

                    return new DalResult<Page>(DalStatus.Success, result);
                }
                catch (Exception e)
                {
                    Log.Error($"Exception getting page {name} of section {parent}. ex: {e.Message}");
                    Log.Debug(e.StackTrace);
                    return new DalResult<Page>(DalStatus.Unknown, null, "Error getting page");
                }
            });
        }

        #endregion

        #region Map Page Queries


        public async Task<DalResult<MapPage[]>> GetAllMapPagesForSection(string id)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var result = db.MapPages.Where(x => x.FK_Section == id).ToArray();

                    return new DalResult<MapPage[]>(DalStatus.Success, result);
                }
                catch (Exception e)
                {
                    Log.Error($"Exception getting map pages for section id {id}. ex: {e.Message}");
                    Log.Debug(e.StackTrace);
                    return new DalResult<MapPage[]>(DalStatus.Unknown, null, "Error getting map pages for section");
                }
            });
        }

        public async Task<DalResult> AddMapPageAsync(MapPage page)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    page.Id = Guid.NewGuid().ToString();
                    page.DateCreated = DateTime.Now;
                    page.DateModified = DateTime.Now;
                    page.Link = $"{page.Link}{page.Id}";

                    if (!page.TryValidate(out var errors))
                    {
                        page.Id = Guid.Empty.ToString();
                        return new DalResult(DalStatus.ConstraintViolation, string.Join(';', errors));
                    }

                    var result = await db.MapPages.AddAsync(page);
                    db.SaveChanges(true);

                    return new DalResult(DalStatus.Success, page.Id);
                }
                catch (Exception e)
                {
                    Log.Error($"Exception saving map page {page.Name}. ex: {e.Message}");
                    Log.Debug(e.StackTrace);
                    return new DalResult(DalStatus.Unknown, "Error saving map page");
                }
            });
        }

        public async Task<DalResult> UpdateMapPageAsync(MapPage page)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    page.DateModified = DateTime.Now;

                    if (!page.TryValidate(out var errors))
                    {
                        page.Id = Guid.Empty.ToString();
                        return new DalResult(DalStatus.ConstraintViolation, string.Join(';', errors));
                    }

                    var result = db.MapPages.Update(page);
                    await db.SaveChangesAsync(true);

                    return new DalResult(DalStatus.Success, page.Id);
                }
                catch (Exception e)
                {
                    Log.Error($"Exception saving map page {page.Name}. ex: {e.Message}");
                    Log.Debug(e.StackTrace);
                    return new DalResult(DalStatus.Unknown, "Error saving map page");
                }
            });
        }

        public async Task<DalResult<MapPage>> GetMapPageAsync(string id)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    var result = await db.MapPages.FindAsync(id);

                    db.Entry(result).Collection(r => r.MapMarkers).Load();
                    
                    return new DalResult<MapPage>(DalStatus.Success, result);
                }
                catch (Exception e)
                {
                    Log.Error($"Exception getting page id {id}. ex: {e.Message}");
                    Log.Debug(e.StackTrace);
                    return new DalResult<MapPage>(DalStatus.Unknown, null, "Error getting page");
                }
            });
        }

        #endregion
    }
}
