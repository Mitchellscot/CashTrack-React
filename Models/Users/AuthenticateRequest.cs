using System.ComponentModel.DataAnnotations;

namespace CashTrack.Models.Users
{
    public class AuthenticateRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string PasswordHash { get; set; }
    }
}