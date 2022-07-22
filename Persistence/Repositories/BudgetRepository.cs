using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class BudgetRepository : IBudgetRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public BudgetRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Budget>> GetAllBudgets(int userId)
        {
            var result = await _dbContext.Budgets
                .Where(b => b.CreatedById == userId)
                .OrderBy(n => n.Name)
                .ToListAsync();

            return result;
        }
        public async Task<Budget> GetBudgetById(int id, int userId)
        {
            var result = await _dbContext.Budgets
                .Where(b => b.CreatedById == userId)
                .FirstOrDefaultAsync(t => t.Id == id);

            return result;
        }
        public async Task CreateBudget(Budget budget)
        {
            await _dbContext.Budgets.AddAsync(budget);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateBudget(Budget budget)
        {
            _dbContext.Update(budget);
            await _dbContext.SaveChangesAsync();
        }
        public async Task DeleteBudget(Budget budget)
        {
            _dbContext.Budgets.Remove(budget);
            await _dbContext.SaveChangesAsync();
        }
    }
}
