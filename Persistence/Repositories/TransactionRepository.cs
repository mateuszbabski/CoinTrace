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
    
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TransactionRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactions(int userId)
        {
            return await _dbContext.Transactions
                .Where(i => i.CreatedById == userId)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsBySearchingForm(int userId,
            int budgetId,
            DateTime dateFrom,
            DateTime dateTo, 
            string searchPhrase)
        {
            return await _dbContext.Transactions
                .Where(t => t.CreatedById == userId)
                .Where(t => t.BudgetId == budgetId)
                .Where(t => t.TransactionDate >= dateFrom)
                .Where(t => t.TransactionDate <= dateTo)
                .Where(u => searchPhrase == null
                                            || (u.Category.ToLower().Contains(searchPhrase.ToLower())
                                            || u.Type.ToLower().Contains(searchPhrase.ToLower())))
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }    
        public async Task<Transaction> GetTransactionById(int id, int userId)
        {
            return await _dbContext.Transactions
                .Where(i => i.CreatedById == userId)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Transaction> CreateTransaction(Transaction transaction)
        {
            await _dbContext.Transactions.AddAsync(transaction);
            await _dbContext.SaveChangesAsync();
            return transaction;
        }

        public async Task UpdateTransaction(Transaction transaction)
        {
            _dbContext.Update(transaction);
            await _dbContext.SaveChangesAsync();
        }
        public async Task DeleteTransaction(Transaction transaction)
        {
            _dbContext.Transactions.Remove(transaction);
            await _dbContext.SaveChangesAsync();
        }
    }
}

