//using CashTrack.Data.Entities;
//using CashTrack.Data.Repositories.UserRepository;
//using Xunit;
//using Xunit.Abstractions;
//using Moq;
//using Microsoft.Extensions.Options;
//using CashTrack.Helpers;

//namespace CashTrack.Tests;

//public class UserServiceShould
//{
//    private readonly IUserService _service;
//    private readonly ITestOutputHelper _output;

//    public UserServiceShould(ITestOutputHelper output, IUserService service)
//    {
//        _service = service;
//        _output = output;
//    }
//    [Fact]
//    public void GenerateAJwtToken()
//    {
//        Mock<IUserService> _service = new Mock<IUserService>(IOptions<AppSettings> appSettings, );
//        var user = new User()
//        {
//            first_name = "Mitchell",
//            last_name = "Scott",
//            email = "Mitchellscott@me.com"
//        };
//        var x = _service.Object.GenerateJwtToken(user);

//        _output.WriteLine(x);
//        Assert.NotNull(x);  
//    }
//}