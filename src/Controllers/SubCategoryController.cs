using CashTrack.Services.SubCategoryService;
using Microsoft.AspNetCore.Mvc;

namespace CashTrack.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SubCategoryController : ControllerBase
    {
        private readonly ISubCategoryService _subCategoryService;

        public SubCategoryController(ISubCategoryService subCategoryService) => _subCategoryService = subCategoryService;


    }
}
