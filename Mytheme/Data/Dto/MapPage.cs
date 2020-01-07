using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mytheme.Data.Dto
{
    public class MapPage : DtoObject
    {
        [Key]
        public Guid Id { get; set; }
        public Guid FK_Section { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public Guid Image { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public bool Enabled { get; set; }

        [NotMapped]
        public List<MapMarker> MapMarkers { get; set; }

        public MapPage()
        {
            MapMarkers = new List<MapMarker>();
        }
    }
}