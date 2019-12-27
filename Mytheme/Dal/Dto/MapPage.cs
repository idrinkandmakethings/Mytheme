using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mytheme.Dal.Dto
{
    public class MapPage : DtoObject
    {
        [Key]
        public string Id { get; set; }
        public string FK_Section { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Link { get; set; }
        [Required]
        public string Image { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }
        [Required]
        public DateTime DateModified { get; set; }

        [Required]
        public bool Enabled { get; set; }

        public List<MapMarker> MapMarkers { get; set; }

        [ForeignKey(nameof(FK_Section))]
        public Section Section { get; set; }

        public MapPage()
        {
            MapMarkers = new List<MapMarker>();
        }
    }
}