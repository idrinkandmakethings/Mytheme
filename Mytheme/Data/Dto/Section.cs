using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DapperExtensions.Mapper;

namespace Mytheme.Data.Dto
{
    public enum SectionType
    {
        Campaign,
        Adventure,
        Section,
        Chapter,
        None 
    }

    public sealed class SectionMapper : ClassMapper<Section>
    {
        public SectionMapper()
        {
            Table("Section ");
            Map(r => r.Id).Column("Id").Key(KeyType.Guid);
            Map(r => r.Parent).Column("Parent");
            Map(r => r.Name).Column("Name");
            Map(r => r.Icon).Column("Icon");
            Map(r => r.Description).Column("Description");
            Map(r => r.SectionType).Column("SectionType");
            Map(r => r.SortOrder).Column("SortOrder");
            Map(r => r.DateCreated).Column("DateCreated");
            Map(r => r.DateModified).Column("DateModified");
            Map(r => r.Enabled).Column("Enabled");
            Map(r => r.Children).Ignore();
            Map(r => r.PageIds).Ignore();
            Map(r => r.MapPageIds).Ignore();
        }
    }

    public class Section : DtoObject
    {
        [Key]
        public Guid Id { get; set; }
        public Guid Parent { get; set; }
        public string Name { get; set; }
        public SectionType SectionType { get; set; }
        public int SortOrder { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public bool Enabled { get; set; }

        public List<Section> Children { get; set; }
        public List<PageLink> PageIds { get; set; }
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
