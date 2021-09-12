using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocalSNMP.Entities
{
    public class AppDbContext : DbContext
    {
        private string _connectionString = "Server=(localdb)\\mssqllocaldb;Database=AppDB;Trusted_Connection=True";
        public DbSet<Machine> Machines { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
