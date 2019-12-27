using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Components;
using Mytheme.Map.Models;

namespace Mytheme.Dal.Dto
{
    public class MapMarker : DtoObject
    {
        [Key]
        public string Id { get; set; }
        public string FK_MapPage { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Content { get; set; }

        [Required] 
        public double Lat { get; set; }
        [Required] 
        public double Lon { get; set; }

        [Required]
        public bool Enabled { get; set; }

        [ForeignKey(nameof(FK_MapPage))]
        public MapPage MapPage { get; set; }

        public LeafletMarker ToLeafletMarker()
        {
            return new LeafletMarker
            {
                Id = Id,
                Lat = Lat,
                Lon = Lon,
                Content = new MarkupString(Content)
            };
        }
    }
}