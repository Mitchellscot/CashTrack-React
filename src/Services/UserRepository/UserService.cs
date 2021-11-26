using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CashTrack.Data.Entities;
using CashTrack.Helpers;
using CashTrack.Models.UserModels;
using BCryptNet = BCrypt.Net.BCrypt;

namespace CashTrack.Data.Services.UserRepository
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;
        private readonly AppDbContext _context;
        private readonly ILogger<UserService> _logger;

        public UserService(
            IOptions<AppSettings> appSettings, IMapper mapper, AppDbContext context, ILogger<UserService> logger)
        {
            this._mapper = mapper;
            this._appSettings = appSettings.Value;
            this._context = context;
            this._logger = logger;
        }

        public async Task<Authentication.Response> AuthenticateAsync(Authentication.Request model)
        {
            var user = await _context.Users.Where(u => u.first_name.ToUpper() == model.Name.ToUpper()).FirstOrDefaultAsync<User>();
            if (user == null || !BCryptNet.Verify(model.Password, user.password_hash))
            {
                return null;
            }
            var response = _mapper.Map<Authentication.Response>(user) with { Token = GenerateJwtToken(user) };
            _logger.LogInformation($"{response.FirstName} logged in at {DateTime.Now}");
            return response;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            var query = _context.Users.Where(u => u.id == id);
            if (query.Any())
            {
                return await query.FirstOrDefaultAsync();
            }
            return null;
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("id", user.id.ToString()),
                    new Claim(ClaimTypes.Name, user.email.ToString()),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
