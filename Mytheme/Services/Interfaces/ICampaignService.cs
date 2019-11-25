using System.Threading.Tasks;
using Mytheme.Dal;
using Mytheme.Dal.Dto;

namespace Mytheme.Services.Interfaces
{
    public interface ICampaignService
    {
        Task<DalResult> AddCampaign(Campaign campaign);
        Task<DalResult> UpdateCampaign(Campaign campaign);
        Task<DalResult<Campaign>> GetCampaign(string id);
        Task<DalResult<Campaign[]>> GetAllCampaigns();

        Task<DalResult> AddAdventure(Adventure adventure);
        Task<DalResult> UpdateAdventure(Adventure adventure);
        Task<DalResult<Adventure>> GetAdventure(string id);
        Task<DalResult<Adventure[]>> GetAllAdventures();

    }
}
