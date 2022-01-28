using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using CashTrack.Models.IncomeModels;
using System;
using CashTrack.Helpers.Exceptions;
using CashTrack.Services.IncomeService;
using Microsoft.AspNetCore.Routing;

namespace CashTrack.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class IncomeController : ControllerBase
    {
        private readonly IIncomeService _service;
        public IncomeController(IIncomeService service) => _service = service;

        [HttpGet]
        public async Task<ActionResult<IncomeResponse>> GetIncomes([FromQuery] IncomeRequest request)
        {
            try
            {
                var response = await _service.GetIncomeAsync(request);
                return response;
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message.ToString() });
            }
        }
    }
}
