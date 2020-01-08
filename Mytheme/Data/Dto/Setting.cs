using System.ComponentModel.DataAnnotations;

namespace Mytheme.Data.Dto
{
    public class Setting
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
