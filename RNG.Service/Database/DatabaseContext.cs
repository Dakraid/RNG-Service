#region header
// RNG.Service
// RNG.Service / DatabaseContext.cs BY Kristian Schlikow
// First modified on 2023.02.23
// Last modified on 2023.02.27
#endregion

namespace RNG.Service.Database
{
#region usings
    using Microsoft.EntityFrameworkCore;

    using Models;
#endregion

    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<RngEntry> RngResults { get; set; }
        public DbSet<BatchedTest> TestResults { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RngEntry>().ToTable("RNG_List");
            modelBuilder.Entity<BatchedTest>().ToTable("RNG_Tests");
        }
    }
}
