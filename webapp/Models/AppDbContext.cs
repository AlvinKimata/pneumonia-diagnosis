using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Identity;


namespace school_project.Models{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {    
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Employee> Employees {get; set;}
        public DbSet<SingleImageDiagnosis> SingleImageDiagnosis {get; set;}
        public DbSet<BatchImageDiagnosis> BatchImageDiagnosis {get; set;}
        public DbSet <Photo> Photo {get; set;}
        public DbSet <ImageRes> ImageRes {get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(e => new { e.LoginProvider, e.ProviderKey });
            modelBuilder.Entity<IdentityUserRole<string>>().HasKey(e => new { e.UserId, e.RoleId});
            modelBuilder.Entity<IdentityUserToken<string>>().HasKey(e => new {e.UserId, e.LoginProvider, e.Name});

            modelBuilder.Entity<BatchImageDiagnosis>()
                .HasMany(b => b.Photos)
                .WithOne();

            modelBuilder.Entity<BatchImageDiagnosis>()  
                .HasMany(b => b.ImagesResults)
                .WithOne();
        }

    }
}

