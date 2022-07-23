using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IBudgetRepository
    {
        Task<IEnumerable<Budget>> GetAllBudgets(int userId);
        Task<Budget> GetBudgetById(int id, int userId);
        Task<Budget> CreateBudget(Budget budget);
        Task DeleteBudget(Budget budget);
        Task UpdateBudget(Budget budget);
    }
}
