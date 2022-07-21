using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Context
{
    public interface IApplicationDbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Budget> Budgets { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}