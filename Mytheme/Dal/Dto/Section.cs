using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mytheme.Dal.Dto
{
    public class Section : DtoObject
    {
        [Key]
        public string Id { get; set; }
        public string Parent { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public SectionType SectionType { get; set; }
        public int Order { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }
        [Required]
        public DateTime DateModified { get; set; }

        [Required]
        public bool Enabled { get; set; }

        [NotMapped]
        public List<Section> Children { get; set; }
        [NotMapped]
        public List<PageLink> PageIds { get; set; }
        [NotMapped]
        public List<PageLink> MapPageIds { get; set; }

        public Section()
        {
            Children = new List<Section>();
            PageIds = new List<PageLink>();
            MapPageIds = new List<PageLink>();
        }
    }

    public enum SectionType
    {
        Campaign,
        Adventure,
        Book,
        Section,
        Chapter,
    }

    public class PageLink
    {
        public string Name { get; set; }
        public string Link { get; set; }

        public PageLink(string name, string link)
        {
            Name = name;
            Link = link;
        }
    }

    public class Page
    {
        [Key]
        public string Id { get; set; }
        public string FK_Section { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Link { get; set; }
        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }
        [Required]
        public DateTime DateModified { get; set; }

        [Required]
        public bool Enabled { get; set; }

        [ForeignKey(nameof(FK_Section))]
        public Section Section { get; set; }

    }

    public class MapPage
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

    public class MapMarker
    {
        [Key]
        public string Id { get; set; }
        public string FK_MapPage { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Content { get; set; }

        [Required]
        public bool Enabled { get; set; }

        [ForeignKey(nameof(FK_MapPage))]
        public MapPage MapPage { get; set; }
    }
}
