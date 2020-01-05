using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DapperExtensions.Mapper;

namespace Mytheme.Data.Dto
{

    public sealed class RandomTableMapper : ClassMapper<RandomTable>
    {
        public RandomTableMapper()
        {
            Table("RandomTable");
            Map(r => r.Id).Column("Id").Key(KeyType.Guid);
            Map(r =>r.Name).Column("Name");
            Map(r => r.Category).Column("Category");
            Map(r => r.Description).Column("Description");
            Map(r => r.Enabled).Column("Enabled");
            Map(r => r.Entries).Ignore();
        }
    }

    public class RandomTable : DtoObject
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }

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
