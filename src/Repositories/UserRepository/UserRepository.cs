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
using CashTrack.Data;
using CashTrack.Helpers.Exceptions;

namespace CashTrack.Repositories.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task<Users[]> GetAllUsers()
        {
            var users = await _context.Users.OrderBy(x => x.id).ToArrayAsync();
            return users;
        }

        public async Task<Users> GetUserByIdAsync(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.id == id);
            if (user == null)
            {
                throw new UserNotFoundException(id.ToString());
            }
            return user;
        }
    }
}
