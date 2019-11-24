using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mytheme.Dal.Dto
{
    public class FileData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }
        [Required]
        public string FileName { get; set; }
        [Required]
        public FileType FileType { get; set; }
    }

    public enum FileType
    {
        Map,
        Image,
    }
}
