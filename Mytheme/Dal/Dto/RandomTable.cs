using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mytheme.Dal.Dto
{
    public class RandomTable
    {
        [Key]
        public string Id { get; set; }

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
        public string FK_RandomTable { get; set; }
        public string Entry { get; set; }
        public int LowerBound { get; set; }
        public int UpperBound { get; set; }
        
        [ForeignKey(nameof(FK_RandomTable))]
        public RandomTable RandomTable { get; set; }
    }

    public class TableCategory
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public bool Enabled { get; set; }
    }
}
