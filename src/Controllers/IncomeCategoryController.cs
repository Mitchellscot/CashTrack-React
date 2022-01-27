using CashTrack.Helpers.Exceptions;
using CashTrack.Models.IncomeCategoryModels;
using CashTrack.Services.SubCategoryService;
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
        public IncomeCategoryController(IIncomeCategoryService service) => _service = service;
    }
}
