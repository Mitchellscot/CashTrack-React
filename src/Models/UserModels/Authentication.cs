namespace CashTrack.Models.UserModels
{
    public class Authentication
    {
        public record Request(string Name, string Password);
        //automapper requires a parameterless constructor. This does the trick.
        public record Response()
        {
            public int Id { get; init; }
            public string FirstName { get; init; }
            public string LastName { get; init; }
            public string Email { get; init; }
            public string Token { get; init; }
        };
    }
}
