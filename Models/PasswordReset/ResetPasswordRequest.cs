using System.ComponentModel.DataAnnotations;

namespace CashTrack.Models.PasswordReset
{
    public class ResetPasswordRequest
    {
        [Required]
        public string Token { get; set; }

        [Required]
        public string Password { get; set; }
    }
}