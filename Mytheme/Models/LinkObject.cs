using System;
using Mytheme.Utility;

namespace Mytheme.Models
{
    public class LinkObject : IComparable<LinkObject>
    {
        public string Name { get; set; }
        public NavigationLink Link { get; set; }

        public LinkObject(string name, NavigationLink link)
        {
            Name = name;
            Link = link;
        }

        public int CompareTo(LinkObject other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return CustomCompare.CompareNatural(Name, other.Name);
        }
    }
}