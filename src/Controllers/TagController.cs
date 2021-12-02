using AutoMapper;
using CashTrack.Models.ExpenseModels;
using CashTrack.Models.TagModels;
using CashTrack.Repositories.TagRepository;
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
        private readonly ITagRepository _tagService;

        public TagController(ILogger<UserController> logger, ITagRepository tagService, IMapper mapper)
        {
            this._mapper = mapper;
            this._logger = logger;
            this._tagService = tagService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllTags()
        { 
            var tags = await _tagService.GetAllTags();
            return Ok(_mapper.Map<Tag[]>(tags));
        }
    }
}
