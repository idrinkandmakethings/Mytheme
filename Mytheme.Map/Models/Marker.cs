using Microsoft.AspNetCore.Components;

namespace Mytheme.Map.Models
{
    public class Marker
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public MarkupString Content { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
    }
}
