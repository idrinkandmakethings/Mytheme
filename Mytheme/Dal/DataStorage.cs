using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
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

        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<Adventure> Adventures { get; set; }
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

            modelBuilder.Entity<RandomTable>().Property(b => b.Id).ValueGeneratedOnAdd().HasValueGenerator<StringGuidValueGenerator>();
            modelBuilder.Entity<RandomTable>().HasAlternateKey(t => t.Name);

            modelBuilder.Entity<TableCategory>().Property(b => b.Id).ValueGeneratedOnAdd().HasValueGenerator<StringGuidValueGenerator>();
            modelBuilder.Entity<TableCategory>().HasAlternateKey(t => t.Name);

            modelBuilder.Entity<Template>().Property(b => b.Id).ValueGeneratedOnAdd().HasValueGenerator<StringGuidValueGenerator>();
            modelBuilder.Entity<Template>().HasAlternateKey(t => t.Name);

            modelBuilder.Entity<TableCategory>().Property(b => b.Id).ValueGeneratedOnAdd().HasValueGenerator<StringGuidValueGenerator>();
            modelBuilder.Entity<TemplateCategory>().HasAlternateKey(t => t.Name);

            modelBuilder.Entity<FileData>().Property(b => b.Id).ValueGeneratedOnAdd().HasValueGenerator<StringGuidValueGenerator>();

            modelBuilder.Entity<Campaign>().Property(b => b.Id).ValueGeneratedOnAdd().HasValueGenerator<StringGuidValueGenerator>();
            modelBuilder.Entity<Adventure>().Property(b => b.Id).ValueGeneratedOnAdd().HasValueGenerator<StringGuidValueGenerator>();
            modelBuilder.Entity<Page>().Property(b => b.Id).ValueGeneratedOnAdd().HasValueGenerator<StringGuidValueGenerator>();
            modelBuilder.Entity<MapPage>().Property(b => b.Id).ValueGeneratedOnAdd().HasValueGenerator<StringGuidValueGenerator>();
            modelBuilder.Entity<MapMarker>().Property(b => b.Id).ValueGeneratedOnAdd().HasValueGenerator<StringGuidValueGenerator>();

        }
    }

    public class StringGuidValueGenerator : ValueGenerator<string>
    {
        public override string Next(EntityEntry entry)
        {
            return Guid.NewGuid().ToString();
        }

        public override bool GeneratesTemporaryValues => false;
    }
}
