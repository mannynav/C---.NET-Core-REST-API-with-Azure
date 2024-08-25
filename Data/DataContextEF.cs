
using DotnetAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DotnetAPI.Data
{
    public class DataContextEF : DbContext
    {
        private readonly IConfiguration _config;

        //Will be instanciated inside controller
        public DataContextEF(IConfiguration config)
        {
            _config = config;
        }

        //Maps model back to table
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserJobInfo> UserJobInfo { get; set; }
        public virtual DbSet<UserSalary> UserSalary { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_config.GetConnectionString("DefaultConnection"),
                optionsBuilder => optionsBuilder.EnableRetryOnFailure());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("TutorialAppSchema");

            modelBuilder.Entity<User>()
            .ToTable("Users", "TutorialAppSchema")
            .HasKey(u => u.UserId);

            modelBuilder.Entity<UserSalary>()
            .HasKey(u => u.UserId);

            modelBuilder.Entity<UserJobInfo>()
            .HasKey(u => u.UserId);

        }

    }



}