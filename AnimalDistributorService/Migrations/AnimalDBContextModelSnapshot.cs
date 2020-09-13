﻿// <auto-generated />
using System;
using AnimalDistributorService.DataAccess.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace AnimalDistributorService.Migrations
{
    [DbContext(typeof(AnimalDBContext))]
    partial class AnimalDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Contract.Models.Animal", b =>
                {
                    b.Property<Guid?>("AnimalId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Alias")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("AnimalTypeRef")
                        .HasColumnType("integer");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.HasKey("AnimalId");

                    b.HasIndex("AnimalTypeRef");

                    b.ToTable("Animal");
                });

            modelBuilder.Entity("Contract.Models.AnimalType", b =>
                {
                    b.Property<int>("AnimalTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("AnimalTypeId");

                    b.ToTable("AnimalType");
                });

            modelBuilder.Entity("Contract.Models.ComputerVision.Rejection", b =>
                {
                    b.Property<Guid>("RejectionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AnimalRef")
                        .HasColumnType("uuid");

                    b.Property<string>("FileName")
                        .HasColumnType("text");

                    b.Property<int>("MediaType")
                        .HasColumnType("integer");

                    b.Property<bool>("Verified")
                        .HasColumnType("boolean");

                    b.HasKey("RejectionId");

                    b.HasIndex("AnimalRef")
                        .IsUnique();

                    b.ToTable("CV_Rejection");
                });

            modelBuilder.Entity("Contract.Models.ComputerVision.StatisticsModel", b =>
                {
                    b.Property<Guid>("StatisticId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AnimalId")
                        .HasColumnType("uuid");

                    b.Property<string>("ElapsedTime")
                        .HasColumnType("text");

                    b.Property<string>("FileName")
                        .HasColumnType("text");

                    b.Property<int>("TypeOfMedia")
                        .HasColumnType("integer");

                    b.HasKey("StatisticId");

                    b.ToTable("CV_Statistics");
                });

            modelBuilder.Entity("Contract.Models.Localization", b =>
                {
                    b.Property<Guid>("LocalizationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AnimalRef")
                        .HasColumnType("uuid");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("LocalizationId");

                    b.HasIndex("AnimalRef")
                        .IsUnique();

                    b.ToTable("Localization");
                });

            modelBuilder.Entity("Contract.Models.Media", b =>
                {
                    b.Property<Guid>("MediaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AnimalRef")
                        .HasColumnType("uuid");

                    b.Property<string>("Caption")
                        .HasColumnType("character varying(450)")
                        .HasMaxLength(450);

                    b.Property<string>("FileName")
                        .HasColumnType("character varying(450)")
                        .HasMaxLength(450);

                    b.Property<int>("FileSize")
                        .HasColumnType("integer");

                    b.Property<int>("MediaType")
                        .HasColumnType("integer");

                    b.HasKey("MediaId");

                    b.HasIndex("AnimalRef");

                    b.ToTable("Media");
                });

            modelBuilder.Entity("Contract.Models.Profile", b =>
                {
                    b.Property<Guid>("ProfileId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AnimalRef")
                        .HasColumnType("uuid");

                    b.Property<string>("Caption")
                        .HasColumnType("character varying(450)")
                        .HasMaxLength(450);

                    b.Property<string>("FileName")
                        .HasColumnType("character varying(450)")
                        .HasMaxLength(450);

                    b.Property<int>("FileSize")
                        .HasColumnType("integer");

                    b.Property<int>("MediaType")
                        .HasColumnType("integer");

                    b.HasKey("ProfileId");

                    b.HasIndex("AnimalRef")
                        .IsUnique();

                    b.ToTable("Profile");
                });

            modelBuilder.Entity("Contract.Models.Animal", b =>
                {
                    b.HasOne("Contract.Models.AnimalType", "AnimalType")
                        .WithMany("Animals")
                        .HasForeignKey("AnimalTypeRef")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Contract.Models.ComputerVision.Rejection", b =>
                {
                    b.HasOne("Contract.Models.Animal", "Animal")
                        .WithOne("Rejection")
                        .HasForeignKey("Contract.Models.ComputerVision.Rejection", "AnimalRef")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Contract.Models.Localization", b =>
                {
                    b.HasOne("Contract.Models.Animal", "Animal")
                        .WithOne("Localization")
                        .HasForeignKey("Contract.Models.Localization", "AnimalRef")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Contract.Models.Media", b =>
                {
                    b.HasOne("Contract.Models.Animal", "Animal")
                        .WithMany("Media")
                        .HasForeignKey("AnimalRef")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Contract.Models.Profile", b =>
                {
                    b.HasOne("Contract.Models.Animal", "Animal")
                        .WithOne("Profile")
                        .HasForeignKey("Contract.Models.Profile", "AnimalRef")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
