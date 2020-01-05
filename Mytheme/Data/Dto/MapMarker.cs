using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;
using Mytheme.Map.Models;

namespace Mytheme.Data.Dto
{
    public class MapMarker : DtoObject
    {
        [Key]
        public Guid Id { get; set; }
        public Guid FK_MapPage { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public bool Enabled { get; set; }

        public LeafletMarker ToLeafletMarker()
        {
            return new LeafletMarker
            {
                Id = Id.ToString(),
                Lat = Lat,
                Lon = Lon,
                Content = new MarkupString(Content)
            };
        }
    }
}