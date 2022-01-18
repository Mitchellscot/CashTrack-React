using AutoMapper;
using CashTrack.Helpers.Exceptions;
using CashTrack.Models.MerchantModels;
using CashTrack.Repositories.MerchantRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CashTrack.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MerchantController : ControllerBase
    {
        private readonly ILogger<MerchantController> _logger;
        private readonly IMerchantRepository _merchantRepository;
        private readonly IMapper _mapper;

        public MerchantController(ILogger<MerchantController> logger, IMerchantRepository merchantRepository, IMapper mapper)
        {
            _logger = logger;
            _merchantRepository = merchantRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<MerchantModels.Response>> GetAllMerchants([FromQuery] MerchantModels.Request request)
        {
            try
            {
                var response = await _merchantRepository.GetMerchantsAsync(request);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"HEY MITCH - ERROR GETTING ALL MERCHANTS {ex.Message}");
                return BadRequest(new { message = ex.Message.ToString() });
            }
        }
        [HttpGet("detail/{id}")]
        public async Task<ActionResult<MerchantModels.MerchantDetail>> GetMerchantDetailById(int id)
        {
            try
            {
                var result = await _merchantRepository.GetMerchantByIdAsync(id);
                return Ok(result);
            }
            catch (MerchantNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
