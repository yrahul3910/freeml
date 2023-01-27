using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MLServer.Domain.Models;
using MLServer.Infra.Data.Mappings;

namespace MLServer.Infra.Data.Context
{
    public class MLServerContext : DbContext
    {
        public MLServerContext(DbContextOptions<MLServerContext> options)
            : base(options)
        {
        }

        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<Job> Jobs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseLazyLoadingProxies();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserAccountMap());
            modelBuilder.ApplyConfiguration(new JobMap());

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            Save();

            return base.SaveChanges();
        }

        private void Save()
        {
            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("CreationDate") != null))
            {
                if (entry.State == EntityState.Added)
                    entry.Property("CreationDate").CurrentValue = DateTime.UtcNow;

                if (entry.State == EntityState.Modified)
                    entry.Property("CreationDate").IsModified = false;
            }

            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("UpdateDate") != null))
            {
                if (entry.State == EntityState.Added)
                    entry.Property("UpdateDate").IsModified = false;

                if (entry.State == EntityState.Modified)
                    entry.Property("UpdateDate").CurrentValue = DateTime.UtcNow;
            }
        }
    }
}