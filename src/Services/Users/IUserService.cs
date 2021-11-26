using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CashTrack.Data.Entities;
using CashTrack.Models.Users;

namespace CashTrack.Data.Services.Users
{
    public interface IUserService
    {
        Task<Authentication.Response> AuthenticateAsync(Authentication.Request model);
        Task<User> GetUserByIdAsync(int id);
    }
}
