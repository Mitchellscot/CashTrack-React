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
        //General
        Task<bool> Commit(); //save changes
        Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest model);
        User GetById(int id);
        Task<User> GetUserById(int id);
        Task<User[]> GetAllUsers();

    }
}
