using Application.DTOs.Transaction;
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
    }
}
