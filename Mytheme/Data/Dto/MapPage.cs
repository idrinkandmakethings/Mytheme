using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DapperExtensions.Mapper;

namespace Mytheme.Data.Dto
{
    public sealed class MapPageMapper : ClassMapper<MapPage>
    {
        public MapPageMapper()
        {
            Table("MapPage");
            Map(r => r.Id).Column("Id").Key(KeyType.Guid);
            Map(r => r.Name).Column("Name");
            Map(r => r.Link).Column("Link");
            Map(r => r.Image).Column("Image");
            Map(r => r.DateCreated).Column("DateCreated");
            Map(r => r.DateModified).Column("DateModified");
            Map(r => r.Enabled).Column("Enabled");
            Map(r => r.MapMarkers).Ignore();
        }
    }

    public class MapPage : DtoObject
    {
        [Key]
        public string Id { get; set; }
        public string FK_Section { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string Image { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public bool Enabled { get; set; }

        public List<MapMarker> MapMarkers { get; set; }

        public MapPage()
        {
            MapMarkers = new List<MapMarker>();
        }
    }
}