using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Mytheme.Dal.Dto;

namespace Mytheme.Dal
{
    public class DataStorage : DbContext
    {
        public DbSet<RandomTable> RandomTables { get; set; }
        public DbSet<TableEntry> TableEntries { get; set; }
        public DbSet<TableCategory> TableCategories { get; set; }

        public DbSet<Template> Templates { get; set; }
        public DbSet<TableEntry> TemplateFields { get; set; }
        public DbSet<TemplateCategory> TemplateCategories { get; set; }

        public DbSet<FileData> FileData { get; set; }

        public DbSet<Section> Sections { get; set; }
        
        public DbSet<Page> Pages { get; set; }
        public DbSet<MapPage> MapPages { get; set; }
        public DbSet<MapMarker> MapMarkers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var basePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Mytheme");
            optionsBuilder.UseSqlite($"Data Source={Path.Combine(basePath, "mytheme.sqlite")}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

             modelBuilder.Entity<RandomTable>().HasAlternateKey(t => t.Name);

             modelBuilder.Entity<TableCategory>().HasAlternateKey(t => t.Name);

            modelBuilder.Entity<Template>().HasAlternateKey(t => t.Name);

            modelBuilder.Entity<TemplateCategory>().HasAlternateKey(t => t.Name);
        }
    }
}
