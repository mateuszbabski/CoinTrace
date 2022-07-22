using Application.DTOs.Budget;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IBudgetService
    {
        Task<IEnumerable<BudgetViewModel>> GetAllBudgetsAsync();
        Task<BudgetViewModel> GetBudgetByIdAsync(int id);
        Task<Budget> CreateBudgetAsync(CreateBudgetRequest request);
        Task<Budget> UpdateBudgetAsync(int id, CreateBudgetRequest request);
        Task DeleteBudgetAsync(int id);
    }
}