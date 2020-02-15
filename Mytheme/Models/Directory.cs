using System;
using System.Collections.Generic;
using Mytheme.Utility;

namespace Mytheme.Models
{
    public class Directory : IComparable<Directory>
    {
        public LinkObject Header { get; set; }
        public bool Expanded { get; set; } = false;
        public List<LinkObject> Links { get; set; }
        public List<Directory> Directories { get; set; }

        public Directory()
        {
            Links = new List<LinkObject>();
            Directories = new List<Directory>();
        }

        public Directory(string name, NavigationLink link)
        {
            Header = new LinkObject(name, link);
            Links = new List<LinkObject>();
            Directories = new List<Directory>();
        }

        public void Toggle()
        {
            Expanded = !Expanded;
        }

        public string GetIcon()
        {
            if (Expanded)
            {
                return "-";
            }

            return "+";
        }

        public int CompareTo(Directory other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return CustomCompare.CompareNatural(Header.Name, other.Header.Name);
        }
    }
}
