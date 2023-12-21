using Microsoft.EntityFrameworkCore;

namespace HubtelCommerce.DAL
{
    public class CommerceDbContext : DbContext
    {
        public CommerceDbContext(DbContextOptions options) : base(options)
        {
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity && (
                        e.State == EntityState.Added
                    || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((BaseEntity)entityEntry.Entity).DateUpdated = DateTime.UtcNow;

                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseEntity)entityEntry.Entity).DateAdded = DateTime.UtcNow;
                }
            }

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public DbSet<Cart> Carts { get; set; } 
        public DbSet<Item> Items { get; set; }
    }
}
