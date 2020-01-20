using System.Collections.Generic;
using Mytheme.Data.Dto;

namespace Mytheme.Models
{
    public class IndexLevel
    {
        public LinkObject LevelHome { get; set; }

        public SectionType Type { get; set; }
        public SectionType SubSectionType { get; set; }
        
        public List<IndexLevel> SubLevels { get; set; }
        public List<LinkObject> Pages { get; set; }
        public List<LinkObject> Maps { get; set; }

        public IndexLevel()
        {
            SubLevels = new List<IndexLevel>();
            Pages = new List<LinkObject>();
            Maps = new List<LinkObject>();
        }
    }

    public class LinkObject {
        public string Name { get; set; }
        public string Link { get; set; }
    }
}
