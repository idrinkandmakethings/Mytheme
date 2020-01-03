using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mytheme.Dal.Dto
{
    public enum SectionType
    {
        Campaign,
        Adventure,
        Section,
        Chapter,
        None 
    }

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

    public class PageLink
    {
        public string Name { get; set; }
        public string Link { get; set; }

        public PageLink(string name, string link)
        {
            Name = name;
            Link = link;
        }

        public PageLink()
        {
            
        }
    }
}
