using CashTrack.Controllers;
using CashTrack.Models.AuthenticationModels;
using CashTrack.Services.AuthenticationService;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace CashTrack.Tests.Controllers
{
    public class AuthenticateControllerShould
    {
        private readonly AuthenticateController _sut;
        private readonly Mock<IAuthenticationService> _service;
        public AuthenticateControllerShould()
        {
            _service = new Mock<IAuthenticationService>();
            _sut = new AuthenticateController(_service.Object);
        }
        [Fact]
        public async Task ReturnAReponse()
        {
            var request = new AuthenticationModels.Request("mitch", "password");
            var result = await _sut.Authenticate(request);
            var viewResult = Assert.IsType<ActionResult<AuthenticationModels.Response>>(result);
            _service.Verify(s => s.AuthenticateAsync(It.IsAny<AuthenticationModels.Request>()), Times.AtLeastOnce());
        }
    }
}
