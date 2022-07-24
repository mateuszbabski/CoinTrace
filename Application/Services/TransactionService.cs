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

        public async Task<Transaction> CreateTransactionAsync(CreateTransactionRequest request)
        {
            var budget = await _budgetRepository.GetBudgetById(request.BudgetId, _userService.UserId);
            if (budget == null)
                throw new NotFoundException($"Budget {request.BudgetId} doesn't exist");
            
            var transaction = _mapper.Map<Transaction>(request);

            var createdTransaction = await _transactionRepository.CreateTransaction(transaction);
            return createdTransaction;
        }
            
            


        //private async Task<Budget> GetBudgetByIdAsync(int budgetId)
        //{
        //    var budget = await _budgetRepository.GetBudgetById(budgetId, _userService.UserId);
        //    if (budget == null)
        //        throw new NotFoundException();
        //    return budget;
        //}



    }
}
