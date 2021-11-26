namespace CashTrack.Models.Users
{
    public class Authentication
    {
        public record Request(string Name, string Password);
        public record Response(int Id, string FirstName, string LastName, string Email, string Token);
    }
}
