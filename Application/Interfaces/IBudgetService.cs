using Application.DTOs.Budget;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IBudgetService
    {
        Task<Budget> CreateBudgetAsync(CreateBudgetRequest request);
        Task DeleteBudgetAsync(int id);
        Task GetAllBudgetsAsync();
        Task<BudgetViewModel> GetBudgetByIdAsync(int id);
        Task UpdateBudgetAsync();
    }
}