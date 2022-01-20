using Microsoft.AspNetCore.Mvc;
using System;

namespace CashTrack.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SubCategoryController : ControllerBase
    {
        public SubCategoryController()
        {
            //need to generate the SubCategoryService.cs
        }

        //[HttpGet]
        //public async Task<ActionResult<SubCategoryModels.Response>> GetAllSubCategories([FromQuery] SubCategoryModels.Request request)
        //{
        //    try
        //    {
        //        var response = await _subCategoryService.GetSubCategoriesAsync(request);
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { message = ex.Message.ToString() });
        //    }
        //}
    }
}
