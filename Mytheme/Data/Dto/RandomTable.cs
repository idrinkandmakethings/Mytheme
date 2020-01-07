using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mytheme.Data.Dto
{


    public class RandomTable : DtoObject
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }

        [NotMapped]
        public List<TableEntry> Entries { get; set; }
    }

    public class TableEntry
    {
        [Key]
        public Guid Id { get; set; }
        public Guid FK_RandomTable { get; set; }
        public string Entry { get; set; }
        public int LowerBound { get; set; }
        public int UpperBound { get; set; }
    }

    public class TableCategory
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool Enabled { get; set; }
    }
}
