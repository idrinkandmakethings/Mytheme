using System.Threading.Tasks;
using Mytheme.Dal;
using Mytheme.Dal.Dto;

namespace Mytheme.Services.Interfaces
{
    public interface ISectionService
    {
        Task<DalResult<Section[]>> GetAllCampaigns();
        Task<DalResult> AddSection(Section section);
        Task<DalResult> UpdateSection(Section section);
        Task<DalResult<Section>> GetSection(string id);
        Task<DalResult<Section[]>> GetAllSectionsForParent(string id);
    }
}
