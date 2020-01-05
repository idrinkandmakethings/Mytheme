using System.ComponentModel.DataAnnotations;

namespace Mytheme.Data.Dto
{
    public class Setting
    {
        [Key]
        public string Id { get; set; }
        public string Value { get; set; }
    }
}
