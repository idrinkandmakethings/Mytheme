using System.Collections.Generic;

namespace Mytheme.Models
{
    public class Directory
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

        public Directory(string name, string link)
        {
            Header = new LinkObject{Name = name, Link = link};
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
    }
}
