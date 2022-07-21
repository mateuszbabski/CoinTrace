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
        Task<Budget> GetBudgetById(int id, int userId);
        Task CreateBudget(Budget budget);
        Task DeleteBudget(Budget budget);
    }
}
