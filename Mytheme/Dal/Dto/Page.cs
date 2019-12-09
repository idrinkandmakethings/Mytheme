using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mytheme.Dal.Dto
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
        public string Id { get; set; }
        public string FK_Section { get; set; }

        [Required]
        public string Name { get; set; }
        [Required] 
        public PageType PageType { get; set; }
        [Required]
        public string Link { get; set; }
        [Required(AllowEmptyStrings = true)]
        public string Content { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }
        [Required]
        public DateTime DateModified { get; set; }

        [Required]
        public bool Enabled { get; set; }

        [ForeignKey(nameof(FK_Section))]
        public Section Section { get; set; }

    }
}