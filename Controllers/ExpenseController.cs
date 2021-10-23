using Microsoft.AspNetCore.Mvc;
using CashTrack.Data.Services.Users;
using Microsoft.AspNetCore.Authorization;
using CashTrack.Data.Entities;
using Microsoft.Extensions.Logging;
using AutoMapper;
using System.Threading.Tasks;
using CashTrack.Models.Expenses;
using CashTrack.Services.Expenses;

namespace CashTrack.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExpenseController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;
        private readonly IExpenseService _expenseService;

        public ExpenseController(ILogger<UserController> logger, IExpenseService expenseService, IMapper mapper)
        {
            this._mapper = mapper;
            this._logger = logger;
            this._expenseService = expenseService;
        }
        [HttpGet] 
        public async Task<ActionResult<Expense[]>> Expenses()
        {
            try
            {
                var response = await _expenseService.GetAllExpenses();
                return Ok(response);
            }
            catch (System.Exception ex)
            {
                _logger.LogInformation($"HEY MITCH - ERROR GETTING ALL EXPENSES {ex.Message}");
                return BadRequest(new { message = ex.Message.ToString() });
            }
        }

    }
}
