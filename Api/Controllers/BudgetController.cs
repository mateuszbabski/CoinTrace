using Application.DTOs.Budget;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class BudgetController : ControllerBase
    {
        private readonly IBudgetService _budgetService;

        public BudgetController(IBudgetService budgetService)
        {
            _budgetService = budgetService;
        }
        [HttpPost]
        public async Task<ActionResult> AddBudget(CreateBudgetRequest request)
        {
            await _budgetService.CreateBudgetAsync(request);
            return Ok(request);
        }
    }
}
