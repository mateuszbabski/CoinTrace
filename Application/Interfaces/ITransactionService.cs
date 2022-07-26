using Application.DTOs.Transaction;
using Application.Wrappers;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Application.Interfaces
{
    public interface ITransactionService
    {
        Task<IEnumerable<TransactionViewModel>> GetAllTransactionsAsync();
        Task<TransactionViewModel> GetTransactionByIdAsync(int id);
        Task<Transaction> CreateTransactionAsync(CreateTransactionRequest request);
        Task DeleteTransactionAsync(int id);
        Task<Transaction> UpdateTransactionAsync(int id, CreateTransactionRequest request);
        Task<PaginatedList<TransactionViewModel>> GetTransactionsBySearchingFormAsync(
                int budgetId,
                DateTime dateFrom,
                DateTime dateTo, 
                string searchPhrase, 
                int pageNumber,
                int pageSize);
    }
}
