using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace CarEnthusiastHub.Models
{
    public class CarEnthusiastDbContext : DbContext
    {
        public CarEnthusiastDbContext(DbContextOptions<CarEnthusiastDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CarCategory> CarCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CarCategory>().HasKey(cc => new { cc.CarId, cc.CategoryId });

            modelBuilder.Entity<CarCategory>()
                .HasOne(cc => cc.Car)
                .WithMany(c => c.CarCategories)
                .HasForeignKey(cc => cc.CarId);

            modelBuilder.Entity<CarCategory>()
                .HasOne(cc => cc.Category)
                .WithMany(cat => cat.CarCategories)
                .HasForeignKey(cc => cc.CategoryId);

            // Seed categories
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, CategoryName = "Sports Car" },
                new Category { CategoryId = 2, CategoryName = "SUV" },
                new Category { CategoryId = 3, CategoryName = "Convertible" },
                new Category { CategoryId = 4, CategoryName = "Sedan" }
            );
        }
    }
}
