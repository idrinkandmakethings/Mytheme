using System;
using System.Linq;
using System.Threading.Tasks;
using Mytheme.Data;
using Mytheme.Data.Dto;
using Mytheme.Services.Interfaces;
using Serilog;

namespace Mytheme.Services
{
    public class MapService : IMapService
    {
        private DataStorage db;

        public MapService(DataStorage db)
        {
            this.db = db;
        }

        public async Task<DalResult<Guid>> AddMapPageAsync(MapPage page)
        {
            try
            {
                page.DateCreated = DateTime.Now;
                page.DateModified = DateTime.Now;
                page.Link = $"{page.Link}{page.Id}";

                var result = await db.MapPage.InsertAsync(page);

                return new DalResult<Guid>(DalStatus.Success, result);
            }
            catch (Exception e)
            {
                Log.Error(e, "Exception saving map page {Name}.", page.Name);
                return new DalResult<Guid>(DalStatus.Unknown, Guid.Empty, "Error saving map page");
            }
        }

        public async Task<DalResult> UpdateMapPageAsync(MapPage page)
        {
            try
            {
                page.DateModified = DateTime.Now;

                var result = await db.MapPage.UpdateAsync(page);

                return new DalResult(result ? DalStatus.Success : DalStatus.Unknown);
            }
            catch (Exception e)
            {
                Log.Error(e, "Exception saving map page {Name}.", page.Name);
                return new DalResult(DalStatus.Unknown, "Error saving map page");
            }
        }

        public async Task<DalResult<MapPage>> GetMapPageAsync(Guid id)
        {
            try
            {
                var result = await db.MapPage.GetAsync(id);

                var markers = await db.MapMarker.GetAllForMapAsync(id);

                result.MapMarkers = markers.ToList();

                return new DalResult<MapPage>(DalStatus.Success, result);
            }
            catch (Exception e)
            {
                Log.Error(e, "Exception getting page id {Id}.", id);
                return new DalResult<MapPage>(DalStatus.Unknown, null, "Error getting page");
            }
        }

        public async Task<DalResult<Guid>> AddMarkerAsync(MapMarker marker)
        {
            try
            {
                var result = await db.MapMarker.InsertAsync(marker);

                return new DalResult<Guid>(DalStatus.Success, result);
            }
            catch (Exception e)
            {
                Log.Error(e, "Exception saving marker {Name}.", marker.Name );
                return new DalResult<Guid>(DalStatus.Unknown, Guid.Empty, "Error saving marker");
            }
        }

        public async Task<DalResult> DeleteMarkerAsync(Guid id)
        {
            try
            {
                var result = await db.MapMarker.DeleteAsync(id);

                return new DalResult(result ? DalStatus.Success : DalStatus.Unknown);
            }
            catch (Exception e)
            {
                Log.Error(e, "Exception deleting marker {Id}.", id);
                return new DalResult<MapPage>(DalStatus.Unknown, null, "Error deleting marker");
            }
        }
    }
}