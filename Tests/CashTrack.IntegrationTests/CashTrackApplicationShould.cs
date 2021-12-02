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

namespace CashTrack.IntegrationTests
{
    public class CashTrackApplicationShould : IClassFixture<TestServerFixture>
    {
        private TestServerFixture _fixture;
        private ITestOutputHelper _output;

        public CashTrackApplicationShould(TestServerFixture fixture, ITestOutputHelper output)
        {
            _fixture = fixture;
            _output = output;
        }
        [Fact]
        public async Task RenderApplication()
        {
            //var response = await client.GetAsync("/api/expense/100");

            //response.EnsureSuccessStatusCode();

            //var responseString = await response.Content.ReadAsStringAsync();
            //_output.WriteLine(responseString);
            //Assert.Contains("\"id\":100", responseString);

            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/user/authenticate");


            postRequest.Content = new StringContent(
                JsonConvert.SerializeObject(new { name = "mitch", password = "password" }), Encoding.UTF8, "application/json"
                );
            var requestContent = await postRequest.Content.ReadAsStringAsync();
            _output.WriteLine(requestContent);
            var postResponse = await _fixture.Client.SendAsync(postRequest);
            postResponse.EnsureSuccessStatusCode();
            var responseString2 = await postResponse.Content.ReadAsStringAsync();
            var responseObject = new { id = "", firstName = "", lastName = "", email = "", token = "" };
            _output.WriteLine(responseString2);
            var userReponse = JsonConvert.DeserializeAnonymousType(responseString2, responseObject);
            userReponse.token.ShouldNotBeEmpty();
            Assert.Contains("mitch", responseString2);

        }
    }
}