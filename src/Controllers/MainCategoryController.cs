using CashTrack.Services.MainCategoriesService;
using Microsoft.AspNetCore.Mvc;

namespace CashTrack.Controllers
{
    public class MainCategoryController : ControllerBase
    {
        private readonly IMainCategoriesService _service;
        public MainCategoryController(IMainCategoriesService mainCategoryService)
        {
            _service = mainCategoryService;
        }
    }
}
