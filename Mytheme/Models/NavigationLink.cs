using System;

namespace Mytheme.Models
{
    public enum ViewType
    {
        Section,
        Page,
        MapPage,
        None,
        Templates,
        Tables
    }

    public class NavigationLink
    {
        public Guid Link { get; set; }
        public ViewType ViewType { get; set; }
        public string Name { get; set; }

        public NavigationLink(string name)
        {
            Link = Guid.Empty;
            ViewType = ViewType.Page;
            Name = name;
        }

        public NavigationLink(Guid link, ViewType viewType)
        {
            Link = link;
            ViewType = viewType;
        }

        public NavigationLink(Guid link, ViewType viewType, string name)
        {
            Link = link;
            ViewType = viewType;
            Name = name;
        }
    }

    
}
