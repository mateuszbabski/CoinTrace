using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Context
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Budget> Budgets { get; set; }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {

        return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            modelBuilder.Entity<User>()
                .Property(x => x.PhoneNumber)
                .IsRequired(false);

            modelBuilder.Entity<Budget>()
                .Property(x => x.Description)
                .IsRequired(false);
        }
    }
}
        



