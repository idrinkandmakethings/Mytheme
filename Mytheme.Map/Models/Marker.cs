using Microsoft.AspNetCore.Components;

namespace Mytheme.Map.Models
{
    public class LeafletMarker
    {
        public string Id { get; set; }
        public MarkupString Content { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
    }
}
