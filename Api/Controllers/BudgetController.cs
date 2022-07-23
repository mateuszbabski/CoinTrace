using Application.DTOs.Budget;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BudgetController : ControllerBase
    {
        private readonly IBudgetService _budgetService;

        public BudgetController(IBudgetService budgetService)
        {
            _budgetService = budgetService;
        }

        [HttpGet]
        public async Task<IEnumerable<BudgetViewModel>> GetAllBudgets()
        {
            var result = await _budgetService.GetAllBudgetsAsync();
            return result;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BudgetViewModel>> GetBudgetById(int id)
        {
            var result = await _budgetService.GetBudgetByIdAsync(id);
            return result;
        }

        [HttpPost]
        public async Task<ActionResult<BudgetViewModel>> AddBudget(CreateBudgetRequest request)
        {
            var result = await _budgetService.CreateBudgetAsync(request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> DeleteBudget(int id)
        {
            await _budgetService.DeleteBudgetAsync(id);
            return Ok($"Budget {id} deleted");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<string>> UpdateBudget(int id, CreateBudgetRequest request)
        {
            await _budgetService.UpdateBudgetAsync(id, request);
            return Ok($"Budget {id} Updated");
        }

    }
}
