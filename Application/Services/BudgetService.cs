using Application.DTOs.Budget;
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
        public async Task<BudgetViewModel> GetBudgetByIdAsync(int id)
        {
            var userId = _userService.UserId;
            var budget = await _budgetRepository.GetBudgetById(id, userId);

            if (budget == null)
                throw new NotFoundException();

            var result = _mapper.Map<BudgetViewModel>(budget);
            return result;
        }
        public async Task<Budget> CreateBudgetAsync(CreateBudgetRequest request)
        {
            var budget = _mapper.Map<Budget>(request);
            budget.CreatedById = _userService.UserId;
            await _budgetRepository.CreateBudget(budget);
            return budget;
        }
        public async Task UpdateBudgetAsync()
        {

        }
        public async Task DeleteBudgetAsync(int id)
        {
            var budget = await _budgetRepository.GetBudgetById(id, _userService.UserId);
            if (budget == null)
                throw new NotFoundException();
            await _budgetRepository.DeleteBudget(budget);
        }
    }
}
