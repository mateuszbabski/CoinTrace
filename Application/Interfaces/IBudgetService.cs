using Application.DTOs.Budget;

namespace Application.Interfaces
{
    public interface IBudgetService
    {
        Task<int> CreateBudgetAsync(CreateBudgetRequest request);
        Task DeleteBudgetAsync();
        Task GetAllBudgetsAsync();
        Task GetBudgetByIdAsync();
        Task UpdateBudgetAsync();
    }
}