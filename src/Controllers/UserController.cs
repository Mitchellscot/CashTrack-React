using Microsoft.AspNetCore.Mvc;
using CashTrack.Models.UserModels;
using Microsoft.AspNetCore.Authorization;
using CashTrack.Data.Entities;
using Microsoft.Extensions.Logging;
using AutoMapper;
using System.Threading.Tasks;
using System;
using CashTrack.Repositories.UserRepository;
using Microsoft.AspNetCore.Http;
using CashTrack.Helpers.Exceptions;

namespace CashTrack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;
        private readonly IUserRepository _userService;

        public UserController(ILogger<UserController> logger, IUserRepository userService, IMapper mapper)
        {
            this._mapper = mapper;
            this._logger = logger;
            this._userService = userService;
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult<Authentication.Response>> Authenticate(Authentication.Request model)
        {
            try
            {
                var response = await _userService.AuthenticateAsync(model);
                if (response == null)
                {
                    return Unauthorized(new { message = "YOU DIDN'T SAY THE MAGIC WORD!" });
                }
                return Ok(response);
            }
            catch (ArgumentNullException)
            {
                return BadRequest(new { message = "Ah ah ah... you didn't say the magic words!" });
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"HEY MITCH - ERROR AUTHENTICATING {ex.Message} {ex.GetType().ToString()} {ex.InnerException} {ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<User.Response>> GetUserById(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                return Ok(_mapper.Map<User.Response>(user));
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
        public async Task<ActionResult<User.Response[]>> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsers();
                return Ok(_mapper.Map<User.Response[]>(users));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}