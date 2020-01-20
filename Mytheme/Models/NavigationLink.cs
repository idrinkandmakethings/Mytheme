using System;

namespace Mytheme.Models
{
    public enum ViewType
    {
        Section,
        Page,
        MapPage,
        None
    }

    public class NavigationLink
    {
        public Guid Link { get; set; }
        public ViewType ViewType { get; set; }

        public NavigationLink(Guid link, ViewType viewType)
        {
            Link = link;
            ViewType = viewType;
        }
    }

    
}
