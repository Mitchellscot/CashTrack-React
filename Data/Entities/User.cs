using System.Text.Json.Serialization;

namespace CashTrack.Data.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        [JsonIgnore]
        public string PasswordHash { get; set; }
        public string ResetToken { get; set; }
    }
}