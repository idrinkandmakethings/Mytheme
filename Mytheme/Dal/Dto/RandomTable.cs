using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mytheme.Dal.Dto
{
    public class RandomTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        [Required]
        public bool Enabled { get; set; }

        public List<TableEntry> Entries { get; set; }
    }

    public class TableEntry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int RandomTableForeignKey { get; set; }
        public string Entry { get; set; }
        
        [ForeignKey("RandomTableForeignKey")]
        public RandomTable RandomTable { get; set; }
    }

    public class TableCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public bool Enabled { get; set; }
    }
}
