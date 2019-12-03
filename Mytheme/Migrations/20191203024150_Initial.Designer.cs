﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mytheme.Dal;

namespace Mytheme.Migrations
{
    [DbContext(typeof(DataStorage))]
    [Migration("20191203024150_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.0");

            modelBuilder.Entity("Mytheme.Dal.Dto.FileData", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("FileType")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("FileData");
                });

            modelBuilder.Entity("Mytheme.Dal.Dto.MapMarker", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("Enabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FK_MapPage")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("FK_MapPage");

                    b.ToTable("MapMarkers");
                });

            modelBuilder.Entity("Mytheme.Dal.Dto.MapPage", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateModified")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Enabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FK_Section")
                        .HasColumnType("TEXT");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Link")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("FK_Section");

                    b.ToTable("MapPages");
                });

            modelBuilder.Entity("Mytheme.Dal.Dto.Page", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateModified")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Enabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FK_Section")
                        .HasColumnType("TEXT");

                    b.Property<string>("Link")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("FK_Section");

                    b.ToTable("Pages");
                });

            modelBuilder.Entity("Mytheme.Dal.Dto.RandomTable", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Category")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Enabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasAlternateKey("Name");

                    b.ToTable("RandomTables");
                });

            modelBuilder.Entity("Mytheme.Dal.Dto.Section", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateModified")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Enabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Icon")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Order")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Parent")
                        .HasColumnType("TEXT");

                    b.Property<int>("SectionType")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Sections");
                });

            modelBuilder.Entity("Mytheme.Dal.Dto.TableCategory", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Enabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasAlternateKey("Name");

                    b.ToTable("TableCategories");
                });

            modelBuilder.Entity("Mytheme.Dal.Dto.TableEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Entry")
                        .HasColumnType("TEXT");

                    b.Property<string>("FK_RandomTable")
                        .HasColumnType("TEXT");

                    b.Property<int>("LowerBound")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UpperBound")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("FK_RandomTable");

                    b.ToTable("TableEntry");
                });

            modelBuilder.Entity("Mytheme.Dal.Dto.Template", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Category")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Enabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("TemplateBody")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasAlternateKey("Name");

                    b.ToTable("Templates");
                });

            modelBuilder.Entity("Mytheme.Dal.Dto.TemplateCategory", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasAlternateKey("Name");

                    b.ToTable("TemplateCategories");
                });

            modelBuilder.Entity("Mytheme.Dal.Dto.TemplateField", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("FK_Template")
                        .HasColumnType("TEXT");

                    b.Property<int>("FieldType")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Order")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TemplateJson")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("Valid")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("VariableName")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("FK_Template");

                    b.ToTable("TemplateField");
                });

            modelBuilder.Entity("Mytheme.Dal.Dto.MapMarker", b =>
                {
                    b.HasOne("Mytheme.Dal.Dto.MapPage", "MapPage")
                        .WithMany("MapMarkers")
                        .HasForeignKey("FK_MapPage");
                });

            modelBuilder.Entity("Mytheme.Dal.Dto.MapPage", b =>
                {
                    b.HasOne("Mytheme.Dal.Dto.Section", "Section")
                        .WithMany()
                        .HasForeignKey("FK_Section");
                });

            modelBuilder.Entity("Mytheme.Dal.Dto.Page", b =>
                {
                    b.HasOne("Mytheme.Dal.Dto.Section", "Section")
                        .WithMany()
                        .HasForeignKey("FK_Section");
                });

            modelBuilder.Entity("Mytheme.Dal.Dto.TableEntry", b =>
                {
                    b.HasOne("Mytheme.Dal.Dto.RandomTable", "RandomTable")
                        .WithMany("Entries")
                        .HasForeignKey("FK_RandomTable");
                });

            modelBuilder.Entity("Mytheme.Dal.Dto.TemplateField", b =>
                {
                    b.HasOne("Mytheme.Dal.Dto.Template", "Template")
                        .WithMany("Fields")
                        .HasForeignKey("FK_Template");
                });
#pragma warning restore 612, 618
        }
    }
}