using System;
using System.ComponentModel.DataAnnotations;

namespace Mytheme.Data.Dto
{
    public class FileData : DtoObject
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string FileName { get; set; }
        [Required]
        public string DisplayName { get; set; }
        [Required]
        public FileType FileType { get; set; }
    }

    public enum FileType
    {
        Map,
        Image,
        Icon,
        File
    }
}
