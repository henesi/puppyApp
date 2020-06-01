using AnimalDistributorService.DataAccess.EntityFramework.Configuration;
using Contract.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace AnimalDistributorService.DataAccess.EntityFramework
{
    public class AnimalDBContext : DbContext
    {
        public AnimalDBContext(DbContextOptions<AnimalDBContext> options) : base(options)
        { }
        public DbSet<Animal> Animals { get; set; }
        public DbSet<Localization> Localization { get; set; }
        public DbSet<AnimalType> AnimalType { get; set; }
        public DbSet<Media> Media { get; set; }
        public DbSet<Profile> Profile { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AnimalConfig());

            modelBuilder.Entity<Localization>().HasKey(x => x.LocalizationId);
            modelBuilder.Entity<Localization>().Property(c => c.City).IsRequired();
            modelBuilder.Entity<Localization>().Property(c => c.Country).IsRequired();
            modelBuilder.Entity<Localization>().Property(c => c.Street).IsRequired();

            modelBuilder.Entity<Animal>().HasOne(a => a.Localization).WithOne(x => x.Animal).HasForeignKey<Localization>(b => b.AnimalRef);
            modelBuilder.Entity<Animal>().HasOne(a => a.AnimalType).WithMany(x => x.Animals).HasForeignKey(x => x.AnimalTypeRef);

            modelBuilder.Entity<Animal>().HasMany(a => a.Media).WithOne(x => x.Animal).HasForeignKey(b => b.AnimalRef);

            modelBuilder.Entity<Profile>().HasKey(x => x.ProfileId);
            modelBuilder.Entity<Animal>().HasOne(a => a.Profile).WithOne(x => x.Animal).HasForeignKey<Profile>(b => b.AnimalRef);

            base.OnModelCreating(modelBuilder);
        }
    }
}
