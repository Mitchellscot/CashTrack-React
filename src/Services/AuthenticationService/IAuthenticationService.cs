using CashTrack.Models.AuthenticationModels;
using System.Threading.Tasks;

namespace CashTrack.Services.AuthenticationServices
{
    public interface IAuthenticationService
    {
        Task<Authentication.Response> AuthenticateAsync(Authentication.Request model);
    }
}
