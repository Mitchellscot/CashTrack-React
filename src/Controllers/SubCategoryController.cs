using CashTrack.Models.SubCategoryModels;
using CashTrack.Services.SubCategoryService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CashTrack.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SubCategoryController : ControllerBase
    {
        private readonly ISubCategoryService _subCategoryService;

        public SubCategoryController(ISubCategoryService subCategoryService) => _subCategoryService = subCategoryService;

        [HttpGet]
        public async Task<ActionResult<SubCategoryModels.Response>> GetAllSubCategories([FromQuery] SubCategoryModels.Request request)
        {
            try
            {
                var categories = await _subCategoryService.GetSubCategoriesAsync(request);
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message.ToString() + ex.InnerException.ToString() });
            }
        }
    }
}
