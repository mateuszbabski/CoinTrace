using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Context
{
    public interface IApplicationDbContext
    {
        DbSet<User> Users { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}