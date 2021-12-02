using Xunit;
using System.Threading.Tasks;
using CashTrack.Data;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.IO;
using Shouldly;
using Xunit.Abstractions;
using Microsoft.Extensions.PlatformAbstractions;
using CashTrack.Models.UserModels;
using System.Net.Http.Headers;
using System.Linq;
using System.Net;
using CashTrack.IntegrationTests.Common;

namespace CashTrack.IntegrationTests
{
    public class UserControllerShould : IClassFixture<TestServerFixture>
    {
        private TestServerFixture _fixture;
        private ITestOutputHelper _output;
        private const string path = "/user/authenticate";

        public UserControllerShould(TestServerFixture fixture, ITestOutputHelper output)
        {
            _output = output;
            _fixture = fixture;

        }

        [Fact]
        public async Task ReturnAnAuthenticatedUser()
        {
            var request = GetAuthenticationRequest() with { Name = "Test" };
            var response = await _fixture.SendPostRequestAsync(path, request);

            var responseBody = JsonConvert.DeserializeObject<Authentication.Response>(await response.Content.ReadAsStringAsync());

            response.EnsureSuccessStatusCode();
            responseBody.Token.ShouldNotBeEmpty();
            responseBody.FirstName.ShouldBe(request.Name);
            PrintRequestAndResponse(request, response);
        }

        [Theory]
        [InlineData("Password")]
        [InlineData("Chewbaca")]
        [InlineData("123456789")]
        public async Task ReturnUnauthorizedWithWrongPassword(string password)
        {
            var request = GetAuthenticationRequest() with { Password = password };
            var response = await _fixture.SendPostRequestAsync(path, request);
            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
            PrintRequestAndResponse(request,
                JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync()));
        }

        [Theory]
        [InlineData("Henry")]
        [InlineData("Lydia")]
        [InlineData("Edward")]
        [InlineData("Arthur")]
        public async Task ReturnUnauthorizedWithWrongUserName(string username)
        {
            var request = GetAuthenticationRequest() with { Name = username };
            var response = await _fixture.SendPostRequestAsync(path, request);
            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
            PrintRequestAndResponse(request,
                JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync()));
        }

        [Theory]
        [EmptyData]
        public async Task ReturnBadRequestWithEmptyPassword(string password)
        {
            var request = GetAuthenticationRequest() with { Password = password };
            var response = await _fixture.SendPostRequestAsync(path, request);
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            PrintRequestAndResponse(request,
                JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync()));
        }

        [Theory]
        [EmptyData]
        public async Task ReturnBadRequestWithEmptyUsername(string username)
        {
            var request = GetAuthenticationRequest() with { Name = username };
            var response = await _fixture.SendPostRequestAsync(path, request);
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            PrintRequestAndResponse(request,
                JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync()));
        }

        private Authentication.Request GetAuthenticationRequest()
        { 
            return new Authentication.Request("Test", "password");
        }

        private void PrintRequestAndResponse(object request, object response)
        { 
            _output.WriteLine(request.ToString());
            _output.WriteLine(response.ToString());
        }
    }
}