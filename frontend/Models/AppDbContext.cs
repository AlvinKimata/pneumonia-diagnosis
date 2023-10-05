using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore;

namespace school_project.Models{
    public class AppDbContext : DbContext
    {    

        public DbSet<Employee> Employees {get; set;}
    }
}