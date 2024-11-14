using Microsoft.EntityFrameworkCore;
using Netherlands_Project.Models;
using System.Collections.Generic;

namespace Netherlands_Project.Data
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


        // datetimestamp and updated at stamp
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
   