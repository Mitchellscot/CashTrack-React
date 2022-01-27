using CashTrack.Helpers.Exceptions;
using CashTrack.Models.IncomeCategoryModels;
using CashTrack.Services.IncomeCategoryService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CashTrack.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class IncomeCategoryController : ControllerBase
    {
        private readonly IIncomeCategoryService _service;
        public IncomeCategoryController(IIncomeCategoryService service) => _service = service;

        [HttpGet]
        public async Task<ActionResult<IncomeCategoryResponse>> GetIncomeCategories([FromQuery] IncomeCategoryRequest request)
        {
            try
            {
                var result = await _service.GetIncomeCategoriesAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
