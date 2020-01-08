using System;
using System.Drawing;
using System.Threading.Tasks;
using Mytheme.Data;
using Mytheme.Data.Dto;
using Mytheme.Models;

namespace Mytheme.Services.Interfaces
{
    public interface ISectionService
    {
        Task<DalResult<Section[]>> GetAllCampaignsAsync();
        Task<DalResult<IndexLevel>> GetCampaignIndex(Guid id);

        Task<DalResult<Guid>> AddSectionAsync(Section section);
        Task<DalResult> UpdateSectionAsync(Section section);
        Task<DalResult<Section>> GetSectionAsync(Guid id, bool populateChildren = false);
        Task<DalResult<Section[]>> GetAllSectionsForParentAsync(Guid id);

        Task<DalResult<Page[]>> GetAllPagesForSection(Guid id);
        Task<DalResult<Guid>> AddPageAsync(Page page);
        Task<DalResult> UpdatePageAsync(Page page);
        Task<DalResult<Page>> GetPageAsync(Guid id);
        Task<DalResult<Page>> GetPageByNameAsync(Guid parent, string name);


        Task<DalResult<MapPage[]>> GetAllMapPagesForSection(Guid id);
    }
}
