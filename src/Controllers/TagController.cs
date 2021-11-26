using AutoMapper;
using CashTrack.Models.Expenses;
using CashTrack.Services.ExpenseRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CashTrack.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;
        private readonly IExpenseService _expenseService;

        public TagController(ILogger<UserController> logger, IExpenseService expenseService, IMapper mapper)
        {
            this._mapper = mapper;
            this._logger = logger;
            this._expenseService = expenseService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllTags()
        { 
            var tags = await _expenseService.GetAllTags();
            return Ok(_mapper.Map<Tag[]>(tags));
        }
    }
}
