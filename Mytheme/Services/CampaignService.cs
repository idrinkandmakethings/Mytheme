using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mytheme.Dal;
using Mytheme.Dal.Dto;
using Mytheme.Services.Interfaces;
using Serilog;

namespace Mytheme.Services
{
    public class CampaignService : ICampaignService
    {
        private readonly DataStorage db;

        public CampaignService(DataStorage db)
        {
            this.db = db;
        }

        
        public async Task<DalResult> AddCampaign(Campaign campaign)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    var result = await db.Campaigns.AddAsync(campaign);
                    db.SaveChanges(true);

                    return new DalResult(DalStatus.Success);
                }
                catch (Exception e)
                {
                    Log.Error($"Exception saving campaign {campaign.Name}. ex: {e.Message}");
                    Log.Debug(e.StackTrace);
                    return new DalResult(DalStatus.Unknown, "Error saving campaign");
                }
            });
        }

        public async Task<DalResult> UpdateCampaign(Campaign campaign)
        {
            return await Task.Run(() =>
            {
                try
                {
                     db.Campaigns.Update(campaign);
                    db.SaveChanges(true);

                    return new DalResult(DalStatus.Success);
                }
                catch (Exception e)
                {
                    Log.Error($"Exception saving campaign {campaign.Name}. ex: {e.Message}");
                    Log.Debug(e.StackTrace);
                    return new DalResult(DalStatus.Unknown, "Error saving campaign");
                }
            });
        }

        public async Task<DalResult<Campaign>> GetCampaign(string id)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    var result = db.Campaigns
                        .Include(t => t.Adventures)
                        .First(x=> x.Id == id);

                    result.CampaignPages = await db.Adventures.FindAsync(id == result.Id);

                    return new DalResult<Campaign>( DalStatus.Success, result);
                }
                catch (Exception e)
                {
                    Log.Error($"Exception getting campaign id {id}. ex: {e.Message}");
                    Log.Debug(e.StackTrace);
                    return new DalResult<Campaign>(DalStatus.Unknown, null,"Error getting campaign");
                }
            });
        }

        public async Task<DalResult<Campaign[]>> GetAllCampaigns()
        {
            return await Task.Run(() =>
            {
                try
                {
                    var result = db.Campaigns.Where(x => x.Enabled).ToArray();

                    return new DalResult<Campaign[]>(DalStatus.Success, result);
                }
                catch (Exception e)
                {
                    Log.Error($"Exception getting all campaigns. ex: {e.Message}");
                    Log.Debug(e.StackTrace);
                    return new DalResult<Campaign[]>(DalStatus.Unknown, null, "Error getting campaigns");
                }
            });
        }

        public async Task<DalResult> AddAdventure(Adventure adventure)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    var result = await db.Adventures.AddAsync(adventure);
                    db.SaveChanges(true);

                    return new DalResult(DalStatus.Success);
                }
                catch (Exception e)
                {
                    Log.Error($"Exception saving adventure {adventure.Name}. ex: {e.Message}");
                    Log.Debug(e.StackTrace);
                    return new DalResult(DalStatus.Unknown, "Error saving adventure");
                }
            });
        }

        public async Task<DalResult> UpdateAdventure(Adventure adventure)
        {
            return await Task.Run(() =>
            {
                try
                {
                    db.Adventures.Update(adventure);
                    db.SaveChanges(true);

                    return new DalResult(DalStatus.Success);
                }
                catch (Exception e)
                {
                    Log.Error($"Exception updating adventure {adventure.Name}. ex: {e.Message}");
                    Log.Debug(e.StackTrace);
                    return new DalResult(DalStatus.Unknown, "Error updating adventure");
                }
            });
        }

        public async Task<DalResult<Adventure>> GetAdventure(string id)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var result = db.Adventures
                        .Include(t => t.Pages)
                        .Include(t => t.MapPages)
                        .First(x => x.Id == id);

                    return new DalResult<Adventure>(DalStatus.Success, result);
                }
                catch (Exception e)
                {
                    Log.Error($"Exception getting campaign id {id}. ex: {e.Message}");
                    Log.Debug(e.StackTrace);
                    return new DalResult<Adventure>(DalStatus.Unknown, null, "Error getting campaign");
                }
            });
        }

        public async Task<DalResult<Adventure[]>> GetAllAdventures()
        {
            throw new System.NotImplementedException();
        }
    }
}
