using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CashTrack.Data.Entities;
using CashTrack.Models.UserModels;

namespace CashTrack.Repositories.UserRepository
{
    public interface IUserRepository
    {
        Task<Users> GetUserByIdAsync(int id);
        Task<Users[]> GetAllUsers();
    }
}
