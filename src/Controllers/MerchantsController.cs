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
    public class MerchantsController : ControllerBase
    {
        private readonly ILogger<MerchantsController> _logger;
        private readonly IMerchantRepository _merchantRepository;
        private readonly IMapper _mapper;

        public MerchantsController(ILogger<MerchantsController> logger, IMerchantRepository merchantRepository, IMapper mapper)
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
        public async Task<ActionResult<MerchantDetail>> GetMerchantDetail(int id)
        {
            try
            {
                var result = await _merchantRepository.GetMerchantDetailAsync(id);
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
        [HttpPost]
        public async Task<ActionResult<AddEditMerchant>> CreateMerchant([FromBody] AddEditMerchant request)
        {
            try
            {
                var result = await _merchantRepository.CreateUpdateMerchant(request);
                //this is location on the UI, it's missing /api from the url, which is fine the user doesn't need the api address.
                return CreatedAtAction("detail", new { id = result.id }, result);
            }
            catch (DuplicateMerchantNameException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateMerchant([FromBody] AddEditMerchant request )
        {
            if (request.Id == null)
                return BadRequest("Need a merchant id in the body of the request.");
            try
            {
                var result = await _merchantRepository.CreateUpdateMerchant(request);
                return Ok();
            }
            catch (DuplicateMerchantNameException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMerchant(int id)
        {
            try
            {
                if(await _merchantRepository.DeleteMerchant(id))
                    return Ok();
                else
                    return BadRequest("Error occured while deleting merchant.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
