using AppCore.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options)
               : base(options)
        {
            Database.Migrate();
        }

        public DbSet<Vacancy> Vacancies { get; set; }

        public DbSet<Respond> Responds { get; set; }

        public DbSet<AppFile> AppFiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Vacancy>()
                .HasIndex(v => new { v.EmploymentType, v.ExperienceType, v.Name, v.Area });
        }
    }
}
