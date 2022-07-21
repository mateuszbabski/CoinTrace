using Application.DTOs.Budget;
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
    public class BudgetService : IBudgetService
    {
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _userService;
        private readonly IBudgetRepository _budgetRepository;

        public BudgetService(IMapper mapper, ICurrentUserService userService, IBudgetRepository budgetRepository)
        {
            _mapper = mapper;
            _userService = userService;
            _budgetRepository = budgetRepository;
        }

        public async Task GetAllBudgetsAsync()
        {

        }
        public async Task GetBudgetByIdAsync()
        {

        }
        public async Task<int> CreateBudgetAsync(CreateBudgetRequest request)
        {
            var budget = _mapper.Map<Budget>(request);
            budget.CreatedById = _userService.UserId;

            await _budgetRepository.CreateBudget(budget);

            return budget.Id;
        }
        public async Task UpdateBudgetAsync()
        {

        }
        public async Task DeleteBudgetAsync()
        {

        }
    }
}
