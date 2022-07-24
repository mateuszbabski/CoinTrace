using Application.Interfaces;
using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Context
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        private readonly ICurrentUserService _userService;

        public ApplicationDbContext(DbContextOptions options, ICurrentUserService userService) : base(options)
        {
            _userService = userService;
        }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<Transaction> Transactions { get; set; }


        public async Task<int> SaveChangesAsync()
        {
            foreach (var entry in ChangeTracker.Entries<AuditableBaseEntity>())
            {
                entry.Entity.CreatedById = _userService.UserId;
            }


            return await base.SaveChangesAsync();
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

            modelBuilder.Entity<Transaction>()
                .Property(x => x.Description)
                .IsRequired(false);

        }
    }
}
            
                
        



