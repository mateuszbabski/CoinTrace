using Application.DTOs.Transaction;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    

    public class TransactionService : ITransactionService
    {
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _userService;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IBudgetRepository _budgetRepository;

        public TransactionService(IMapper mapper, ICurrentUserService userService, ITransactionRepository transactionRepository, IBudgetRepository budgetRepository)
        {
            _mapper = mapper;
            _userService = userService;
            _transactionRepository = transactionRepository;
            _budgetRepository = budgetRepository;
        }
        public async Task<IEnumerable<TransactionViewModel>> GetAllTransactionsAsync()
        {
            var transactions = await _transactionRepository.GetAllTransactions(_userService.UserId);
            if (transactions == null)
                throw new NotFoundException();

            var mappedTransactionList = _mapper.Map<IEnumerable<TransactionViewModel>>(transactions);

            return mappedTransactionList;

        }

        public async Task<TransactionViewModel> GetTransactionByIdAsync(int id)
        {
            var transaction = await _transactionRepository.GetTransactionById(id, _userService.UserId);
            if (transaction == null)
                throw new NotFoundException();

            var mappedTransaction = _mapper.Map<TransactionViewModel>(transaction);

            return mappedTransaction;
        }

        public async Task<Transaction> CreateTransactionAsync(CreateTransactionRequest request)
        {
            var budget = await _budgetRepository.GetBudgetById(request.BudgetId, _userService.UserId);
            if (budget == null)
                throw new NotFoundException($"Budget {request.BudgetId} doesn't exist");
            
            var transaction = _mapper.Map<Transaction>(request);

            var createdTransaction = await _transactionRepository.CreateTransaction(transaction);
            return createdTransaction;
        }

        public async Task DeleteTransactionAsync(int id)
        {
            var transaction = await _transactionRepository.GetTransactionById(id, _userService.UserId);
            if (transaction == null)
                throw new NotFoundException();

            await _transactionRepository.DeleteTransaction(transaction);
        }

        public async Task<Transaction> UpdateTransactionAsync(int id, CreateTransactionRequest request)
        {
            var budget = await _budgetRepository.GetBudgetById(request.BudgetId, _userService.UserId);
            var transaction = await _transactionRepository.GetTransactionById(id, _userService.UserId);
            if (transaction == null || budget == null)
                throw new NotFoundException();

            transaction.BudgetId = request.BudgetId;
            transaction.Type = request.Type;
            transaction.Category = request.Category;
            transaction.Value = request.Value;
            transaction.TransactionDate = request.TransactionDate;
            transaction.Description = request.Description;

            await _transactionRepository.UpdateTransaction(transaction);

            return transaction;
        }



    }
}
