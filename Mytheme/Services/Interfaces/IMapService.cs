using System;
using System.Threading.Tasks;
using Mytheme.Data;
using Mytheme.Data.Dto;

namespace Mytheme.Services.Interfaces
{
    public interface IMapService
    {
        Task<DalResult<Guid>> AddMapPageAsync(MapPage page);
        Task<DalResult> UpdateMapPageAsync(MapPage page);
        Task<DalResult<MapPage>> GetMapPageAsync(Guid id);
        Task<DalResult<Guid>> AddMarkerAsync(MapMarker marker);
        Task<DalResult> DeleteMarkerAsync(Guid id);
    }
}