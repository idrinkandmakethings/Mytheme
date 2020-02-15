using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Mytheme.Models;
using Mytheme.Utility;

namespace Mytheme.Data.Dto
{
    public enum SectionType
    {
        Campaign,
        Adventure,
        Chapter,
        None 
    }


    public class Section : DtoObject, IComparable<Section>
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

        [NotMapped]
        public List<Section> Children { get; set; }
        [NotMapped]
        public List<LinkObject> PageIds { get; set; }
        [NotMapped]
        public List<LinkObject> MapPageIds { get; set; }

        public Section()
        {
            Children = new List<Section>();
            PageIds = new List<LinkObject>();
            MapPageIds = new List<LinkObject>();
        }

        public int CompareTo(Section other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return CustomCompare.CompareNatural(Name, other.Name);
        }
    }
}
