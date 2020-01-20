namespace Mytheme.Models
{
    public class LinkObject {
        public string Name { get; set; }
        public NavigationLink Link { get; set; }

        public LinkObject(string name, NavigationLink link)
        {
            Name = name;
            Link = link;
        }
    }
}