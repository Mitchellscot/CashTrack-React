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

namespace CashTrack.IntegrationTests
{
    public class UserControllerShould : IClassFixture<TestServerFixture>
    {
        private TestServerFixture _fixture;
        private ITestOutputHelper _output;

        public UserControllerShould(TestServerFixture fixture, ITestOutputHelper output)
        {
            _fixture = fixture;
            _output = output;
        }
        [Fact]
        public async Task RenderApplication()
        {


            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/user/authenticate");

            postRequest.Content = new StringContent(JsonConvert.SerializeObject(new Authentication.Request("Test", "password")));
            postRequest.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var requestContent = await postRequest.Content.ReadAsStringAsync();
            _output.WriteLine(requestContent);
            var postResponse = await _fixture.Client.SendAsync(postRequest);
            postResponse.EnsureSuccessStatusCode();
            var responseString2 = await postResponse.Content.ReadAsStringAsync();
            var responseObject = new { id = "", firstName = "", lastName = "", email = "", token = "" };
            _output.WriteLine(responseString2);
            var userReponse = JsonConvert.DeserializeAnonymousType(responseString2, responseObject);
            userReponse.token.ShouldNotBeEmpty();
        }
    }
}