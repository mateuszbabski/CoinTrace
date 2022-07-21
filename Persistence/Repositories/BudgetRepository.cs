using Application.Interfaces;
using Domain.Entities;
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

        public async Task GetAllBudgets()
        {

        }
        public async Task GetBudgetById()
        {

        }
        public async Task CreateBudget(Budget budget)
        {
            await _dbContext.Budgets.AddAsync(budget);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateBudget()
        {

        }
        public async Task DeleteBudget()
        {

        }
    }
}
