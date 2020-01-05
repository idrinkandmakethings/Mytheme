using System;
using System.ComponentModel.DataAnnotations;

namespace Mytheme.Data.Dto
{
    public enum PageType
    {
        Person,
        Place,
        Thing,
        Rules,
        None
    }

    public class Page : DtoObject
    {
        [Key]
        public Guid Id { get; set; }
        public Guid FK_Section { get; set; }
        public string Name { get; set; }
        public PageType PageType { get; set; }
        public string Link { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public bool Enabled { get; set; }
    }
}