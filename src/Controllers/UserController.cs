using Microsoft.AspNetCore.Mvc;
using CashTrack.Models.UserModels;
using Microsoft.Extensions.Logging;
using AutoMapper;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;
using CashTrack.Helpers.Exceptions;
using CashTrack.Services.UserService;

namespace CashTrack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, IUserService userService, IMapper mapper)
        {
            this._mapper = mapper;
            this._logger = logger;
            this._userService = userService;
        }
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<UserModels.Response>> GetUserById(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                return Ok(_mapper.Map<UserModels.Response>(user));
            }
            catch (UserNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet("all")]
        [Authorize]
        public async Task<ActionResult<UserModels.Response[]>> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                return Ok(_mapper.Map<UserModels.Response[]>(users));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}