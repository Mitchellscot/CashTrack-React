using System.ComponentModel.DataAnnotations;

namespace CashTrack.Models.PasswordReset
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}