using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mytheme.Data;
using Mytheme.Data.Dto;
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
            try
            {
                var result = await db.GetAllCampaigns();
                return new DalResult<Section[]>(DalStatus.Success, result);
            }
            catch (Exception e)
            {
                Log.Error(e,"Exception getting campaign list.");
                return new DalResult<Section[]>(DalStatus.Unknown, null, "Error saving section");
            }
        }

        public async Task<DalResult<Directory>> GetCampaignDirectory(Guid id)
        {
            var result = await GetDirectoryForSection(id);
            return new DalResult<Directory>(DalStatus.Success, result);
        }

        private async Task<Directory> GetDirectoryForSection(Guid id)
        {
            try
            {
                var section = await db.Section.GetAsync(id);
                var currentSubType = section.SectionType.GetSubSectionType();

                var directory = new Directory(section.Name, new NavigationLink(section.Id, ViewType.Section));

                var maps = await db.MapPage.GetAllForSectionAsync(id);

                var mapsDirectory = new Directory("Maps", new NavigationLink(Guid.Empty, ViewType.None));
                foreach (var mapPage in maps)
                {
                    mapsDirectory.Links.Add(new LinkObject(mapPage.Name, new NavigationLink(mapPage.Id, ViewType.MapPage)));
                }
                mapsDirectory.Links.Sort();

                directory.Directories.Add(mapsDirectory);

                var pages = await db.Page.GetAllForSectionAsync(id);
                var pageDirectory = new Directory("Pages", new NavigationLink(Guid.Empty, ViewType.None));
                foreach (var page in pages)
                {
                    pageDirectory.Links.Add(new LinkObject(page.Name, new NavigationLink(page.Id, ViewType.Page)));
                }
                pageDirectory.Links.Sort();

                directory.Directories.Add(pageDirectory);

                if (currentSubType == SectionType.None)
                {
                    return directory;
                }
                else
                {
                    var subs = await db.Section.GetAllSubSectionsAsync(id);

                    foreach (var sub in subs)
                    {
                        directory.Directories.Add(await GetDirectoryForSection(sub.Id));
                    }

                    directory.Directories.Sort();
                }

                return directory;

            }
            catch (Exception e)
            {
                Log.Error(e, "Exception directory for section {Id}.", id);
                return null;
            }
        }

        public async Task<DalResult<IndexLevel>> GetCampaignIndex(Guid id)
        {
            var result = await GetIndexLevelForSection(id);
            return new DalResult<IndexLevel>(DalStatus.Success, result);
        }

        private async Task<IndexLevel> GetIndexLevelForSection(Guid id)
        {
            try
            {
                var section = await db.Section.GetAsync(id);

                var result = new IndexLevel
                {
                    LevelHome = new LinkObject(section.Name, new NavigationLink(section.Id, ViewType.Section)),
                    Type = section.SectionType,
                    SubSectionType = section.SectionType.GetSubSectionType()
                };

                var maps = await db.MapPage.GetAllForSectionAsync(id);

                result.Maps = maps.Select(x => new LinkObject( x.Name,  new NavigationLink(x.Id, ViewType.MapPage))).ToList();

                var pages = await db.Page.GetAllForSectionAsync(id);

                result.Pages = pages.Select(x => new LinkObject(x.Name, new NavigationLink(x.Id, ViewType.Page))).ToList();

                result.SubLevels = new List<IndexLevel>();

                if (result.SubSectionType == SectionType.None)
                {
                    return result;
                }
                else
                {
                    var subs = await db.Section.GetAllSubSectionsAsync(id);

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

        public async Task<DalResult<Guid>> AddSectionAsync(Section section)
        {
            try
            {
                section.DateCreated = DateTime.Now;
                section.DateModified = DateTime.Now;

                //if (!section.TryValidate(out var errors))
                //{
                //    section.Id = Guid.Empty.ToString();
                //    return new DalResult(DalStatus.ConstraintViolation, string.Join(';', errors));
                //}

                var id = await db.Section.InsertAsync(section);
                
                return new DalResult<Guid>(DalStatus.Success, id);
            }
            catch (Exception e)
            {
                Log.Error(e,"Exception saving section {Name}.", section.Name);
                return new DalResult<Guid>(DalStatus.Unknown,  Guid.Empty, "Error saving section");
            }
        }

        public async Task<DalResult> UpdateSectionAsync(Section section)
        {
            try
            {
                section.DateModified = DateTime.Now;

                var result = await db.Section.UpdateAsync(section);
                
                return new DalResult(result ? DalStatus.Success : DalStatus.Unknown);
            }
            catch (Exception e)
            {
                Log.Error(e,"Exception saving section {Name}.", section.Name);
                return new DalResult(DalStatus.Unknown, "Error saving section");
            }
        }

        public async Task<DalResult<Section>> GetSectionAsync(Guid id, bool populateChildren = false)
        {
            try
            {
                var result = await db.Section.GetAsync(id);

                if (populateChildren)
                {
                    var childResult = await GetAllSectionsForParentAsync(id);
                    result.Children = childResult.Result.ToList();
                    result.Children.Sort();
                }
                else
                {
                    var subsections = await db.Section.GetAllSubSectionsAsync(id);
                    result.Children = subsections.ToList();
                    result.Children.Sort();
                }
                
                result.PageIds = await db.Page.GetAllLinkObjectsForSectionAsync(id);
                result.PageIds.Sort();

                result.MapPageIds = await db.MapPage.GetAllLinkObjectsForSectionAsync(id);
                result.MapPageIds.Sort();

                return new DalResult<Section>(DalStatus.Success, result);
            }
            catch (Exception e)
            {
                Log.Error(e,"Exception getting section id {Id}.", id);
                return new DalResult<Section>(DalStatus.Unknown, null, "Error getting section");
            }
        }

        public async Task<DalResult<Section[]>> GetAllSectionsForParentAsync(Guid id)
        {
            try
            {
                var result = await db.Section.GetAllSubSectionsAsync(id);

                foreach (var section in result)
                {
                    var childResult = await GetAllSectionsForParentAsync(id);

                    section.Children = childResult.Result.ToList();
                    section.Children.Sort();

                    section.PageIds = await db.Page.GetAllLinkObjectsForSectionAsync(id);
                    section.PageIds.Sort();
                    
                    section.MapPageIds = await db.MapPage.GetAllLinkObjectsForSectionAsync(id);
                    section.MapPageIds.Sort();
                }

                return new DalResult<Section[]>(DalStatus.Success, result);
            }
            catch (Exception e)
            {
                Log.Error(e, "Exception getting all sections.");
                return new DalResult<Section[]>(DalStatus.Unknown, null, "Error getting sections");
            }
        }

        #region Page Queries


        public async Task<DalResult<Page[]>> GetAllPagesForSection(Guid id)
        {
            try
            {
                var result = await db.Page.GetAllForSectionAsync(id);

                return new DalResult<Page[]>(DalStatus.Success, result);
            }
            catch (Exception e)
            {
                Log.Error(e,"Exception getting pages for section id {Id}.", id);
                return new DalResult<Page[]>(DalStatus.Unknown, null, "Error getting pages for section");
            }
        }

        public async Task<DalResult<Guid>> AddPageAsync(Page page)
        {
            try
            {
                page.DateCreated = DateTime.Now;
                page.DateModified = DateTime.Now;

                var result = await db.Page.InsertAsync(page);
                
                return new DalResult<Guid>(DalStatus.Success, result);
            }
            catch (Exception e)
            {
                Log.Error(e,"Exception saving page {Name}.", page.Name);
                return new DalResult<Guid>(DalStatus.Unknown, Guid.Empty, "Error saving page");
            }
        }

        public async Task<DalResult> UpdatePageAsync(Page page)
        {
            try
            {
                page.DateModified = DateTime.Now;
                
                var result = await db.Page.UpdateAsync(page);
                
                return new DalResult(result ? DalStatus.Success : DalStatus.Unknown);
            }
            catch (Exception e)
            {
                Log.Error(e,"Exception saving page {Name}.", page.Name);
                return new DalResult(DalStatus.Unknown, "Error saving page");
            }
        }

        public async Task<DalResult<Page>> GetPageAsync(Guid id)
        {
            try
            {
                var result = await db.Page.GetAsync(id);

                return new DalResult<Page>(DalStatus.Success, result);
            }
            catch (Exception e)
            {
                Log.Error(e,"Exception getting page id {Id}.", id);
                return new DalResult<Page>(DalStatus.Unknown, null, "Error getting page");
            }
        }

        public async Task<DalResult<Page>> GetPageByNameAsync(Guid parent, string name)
        {
            try
            {
                var result = await db.Page.GetByNameAsync(parent, name);

                return new DalResult<Page>(DalStatus.Success, result);
            }
            catch (Exception e)
            {
                Log.Error(e,"Exception getting page {Name} of section {Parent}.", name, parent);
                return new DalResult<Page>(DalStatus.Unknown, null, "Error getting page");
            }
        }

        #endregion

        #region Map Page Queries


        public async Task<DalResult<MapPage[]>> GetAllMapPagesForSection(Guid id)
        {
            try
            {
                var result = await db.MapPage.GetAllForSectionAsync(id);

                return new DalResult<MapPage[]>(DalStatus.Success, result);
            }
            catch (Exception e)
            {
                Log.Error(e,"Exception getting map pages for section id {Id}.", id);
                return new DalResult<MapPage[]>(DalStatus.Unknown, null, "Error getting map pages for section");
            }
        }

       

        #endregion
    }
}
