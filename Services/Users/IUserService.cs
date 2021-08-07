using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CashTrack.Data.Entities;
using CashTrack.Models.Authentication;
using CashTrack.Models.PasswordReset;

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
        //not implemented yet. Will have to create files for the Task<Response> like above.
/*      void ForgotPassword(ForgotPasswordRequest model, string origin);
        void ResetPassword(ResetPasswordRequest model);
        void UpdateUser(User model);
        void AddUser(User model);
        void DeleteUser(int id); */

    }
}
