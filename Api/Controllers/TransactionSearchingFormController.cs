using Application.DTOs.Transaction;
using Application.Interfaces;
using Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public class TransactionSearchingFormController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionSearchingFormController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet(Name = "ResultBySearchingForm")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaginatedList<TransactionViewModel>>> GetTransactionsBySearchingForm([FromQuery] RequestParams request)
        {
            var transactionList = await _transactionService.GetTransactionsBySearchingFormAsync(
                request.BudgetId,
                request.DateFrom,
                request.DateTo,
                request.SearchPhrase,
                request.PageNumber,
                request.PageSize);

            return Ok(transactionList);
        }
    }
}
