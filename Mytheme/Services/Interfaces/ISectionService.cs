using System.Threading.Tasks;
using Mytheme.Dal;
using Mytheme.Dal.Dto;

namespace Mytheme.Services.Interfaces
{
    public interface ISectionService
    {
        Task<DalResult<Section[]>> GetAllCampaignsAsync();
        Task<DalResult> AddSectionAsync(Section section);
        Task<DalResult> UpdateSectionAsync(Section section);
        Task<DalResult<Section>> GetSectionAsync(string id);
        Task<DalResult<Section[]>> GetAllSectionsForParentAsync(string id);
    }
}
