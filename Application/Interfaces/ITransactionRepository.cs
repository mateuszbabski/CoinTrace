using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ITransactionRepository
    {
        Task<IEnumerable<Transaction>> GetAllTransactions(int userId);
        Task<Transaction> GetTransactionById(int id, int userId);
        Task<Transaction> CreateTransaction(Transaction transaction);
        Task DeleteTransaction(Transaction transaction);
        Task UpdateTransaction(Transaction transaction);
        Task<IEnumerable<Transaction>> GetTransactionsBySearchingForm(int userId,
            int budgetId,
            DateTime dateFrom,
            DateTime dateTo,
            string searchPhrase);
    }
}

