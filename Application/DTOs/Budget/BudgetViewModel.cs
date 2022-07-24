using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Budget
{
    public class BudgetViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int CreatedById { get; set; }
        public List<Domain.Entities.Transaction> Transactions { get; set; }
    }
}
