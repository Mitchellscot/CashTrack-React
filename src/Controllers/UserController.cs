using Microsoft.AspNetCore.Mvc;
using CashTrack.Data.Services.UserRepository;
using CashTrack.Models.Users;
using Microsoft.AspNetCore.Authorization;
using CashTrack.Data.Entities;
using Microsoft.Extensions.Logging;
using AutoMapper;
using System.Threading.Tasks;
using System;

namespace CashTrack.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
                return BadRequest(new { message = ex.Message.ToString() });
            }
        }
    }
}