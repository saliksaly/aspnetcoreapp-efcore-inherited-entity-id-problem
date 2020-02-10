using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace aspnetcoreapp_efcore_inherited_entity_id_problem.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<AnimalBase> Animal { get; set; }
        public DbSet<Cat> Cat { get; set; }
        public DbSet<Dog> Dog { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed data
            modelBuilder.Entity<Cat>().HasData(
                new Cat
                {
                    Id = 1,
                    Name = "Mourek",
                },
                new Cat
                {
                    Id = 2,
                    Name = "Líza",
                }
            );
        }
    }
}
