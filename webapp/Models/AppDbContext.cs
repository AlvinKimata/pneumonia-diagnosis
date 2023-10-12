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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BatchImageDiagnosis>()
                .HasMany(b => b.Photos)
                .WithOne();

            modelBuilder.Ignore <IdentityUserLogin<string>>();
            modelBuilder.Ignore <IdentityUserRole<string>>();
            modelBuilder.Ignore<IdentityUserClaim<string>>();
            modelBuilder.Ignore<IdentityUserToken<string>>();
            modelBuilder.Ignore<IdentityUser<string>>();
            modelBuilder.Ignore<ApplicationUser>();
        }

    }
}
