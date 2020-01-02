using System.Threading.Tasks;
using Mytheme.Dal;
using Mytheme.Dal.Dto;
using Mytheme.Models;

namespace Mytheme.Services.Interfaces
{
    public interface ISectionService
    {
        Task<DalResult<Section[]>> GetAllCampaignsAsync();
        Task<DalResult<IndexLevel>> GetCampaignIndex();

        Task<DalResult> AddSectionAsync(Section section);
        Task<DalResult> UpdateSectionAsync(Section section);
        Task<DalResult<Section>> GetSectionAsync(string id);
        Task<DalResult<Section[]>> GetAllSectionsForParentAsync(string id);

        Task<DalResult<Page[]>> GetAllPagesForSection(string id);
        Task<DalResult> AddPageAsync(Page page);
        Task<DalResult> UpdatePageAsync(Page page);
        Task<DalResult<Page>> GetPageAsync(string id);
        Task<DalResult<Page>> GetPageByNameAsync(string parent, string name);


        Task<DalResult<MapPage[]>> GetAllMapPagesForSection(string id);
        Task<DalResult> AddMapPageAsync(MapPage page);
        Task<DalResult> UpdateMapPageAsync(MapPage page);
        Task<DalResult<MapPage>> GetMapPageAsync(string id);
    }
}
