using Mytheme.Data.Dto;

namespace Mytheme.Data.Dal
{
    public class MapPageDal : BaseDal<MapPage>
    {
        public MapPageDal(string connectionString) : base(connectionString)
        {
        }
    }

    public class MapMarkerDal : BaseDal<MapMarker>
    {
        public MapMarkerDal(string connectionString) : base(connectionString)
        {
        }
    }
}
