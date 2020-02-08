using System.ComponentModel.DataAnnotations;

namespace Mytheme.Data.Dto
{
    public class Tag : DtoObject
    {
        [Key]
        public int Id { get; set; }
        [Required] 
        public string Value { get; set; }
    }
}
