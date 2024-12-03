using Microsoft.EntityFrameworkCore;
using VitalMetrics.Models;
using System.Collections.Generic;

namespace VitalMetrics.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options)
        : base(options)
        {
        }

        public DbSet<Earheartbeat> Earheartbeats { get; set; }
        public DbSet<FHeartbeat> Fingerheartbeats { get; set; }
        public DbSet<Oxilevel> OxygenLevel { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Accelerometer> Accelerometer { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<Oxilevel>()
                .HasOne<User>() // Relationship to User
                .WithMany(u => u.Oxilevels)
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey(o => o.UserId);

            modelBuilder.Entity<Accelerometer>()
                .HasOne<User>()
                .WithMany(u => u.Accelerometers)
                .OnDelete(DeleteBehavior.Cascade)
             .HasForeignKey(a => a.UserId);

            modelBuilder.Entity<FHeartbeat>()
                .HasOne<User>()
                .WithMany(u => u.FingerHeartbeats)
                .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        }
         
        public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is Common &&
                           (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                var now = DateTime.UtcNow;

                if (entityEntry.State == EntityState.Added)
                {
                    ((Common)entityEntry.Entity).CreatedAt = now; // Set CreatedAt for new entities
                }

                ((Common)entityEntry.Entity).UpdatedAt = now; // Always set UpdatedAt
            }

            return base.SaveChanges();
        }
        // Override SaveChangesAsync method to set CreatedAt and UpdatedAt automatically (for async operations)
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is Common &&
                           (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                var now = DateTime.UtcNow;

                if (entityEntry.State == EntityState.Added)
                {
                    ((Common)entityEntry.Entity).CreatedAt = now; // Set CreatedAt for new entities
                } 

                ((Common)entityEntry.Entity).UpdatedAt = now; // Always set UpdatedAt
            }

            return await base.SaveChangesAsync(cancellationToken);
        }


    }

}
   