using Microsoft.EntityFrameworkCore;
using Mytheme.Dal.Dto;

namespace Mytheme.Dal
{
    public class DataStorage : DbContext
    {
        public DbSet<RandomTable> RandomTables { get; set; }
        public DbSet<TableEntry> TableEntries { get; set; }
        public DbSet<TableCategory> TableCategories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=db.sqlite");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RandomTable>().HasAlternateKey(t => t.Name);
            modelBuilder.Entity<TableCategory>().HasAlternateKey(t => t.Name);
        }
    }
}
