using Xunit;
using Microsoft.Net;
using Microsoft.Net.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit.Abstractions;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using CashTrack.Data;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace CashTrack.IntegrationTests
{
    public class CashTrackApplicationShould
    {
        private readonly ITestOutputHelper _output;

        public CashTrackApplicationShould(ITestOutputHelper output)
        {
            _output = output;
        }
        [Fact]
        public async Task RenderApplication()
        {
            var builder = new WebHostBuilder()
                .UseContentRoot(@"E:\Code\CashTrack\src\")
                .UseEnvironment("Development")
                .UseConfiguration(new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build())
                .ConfigureServices(services =>
                {
                    services.AddDbContext<AppDbContext>(options =>
                    {
                        options.UseNpgsql("Host=localhost;Port=5432;Username=postgres;Password=password;Database=cash_track;");
                    });
                })
                .UseStartup<CashTrack.Startup>();

            var server = new TestServer(builder);

            var client = server.CreateClient();
            //var response = await client.GetAsync("/api/expense/100");

            //response.EnsureSuccessStatusCode();

            //var responseString = await response.Content.ReadAsStringAsync();
            //_output.WriteLine(responseString);
            //Assert.Contains("\"id\":100", responseString);

            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/user/authenticate");


            postRequest.Content = new StringContent(
                JsonConvert.SerializeObject(new { name = "mitchell", password = "password" }), Encoding.UTF8, "application/json"
                );
            var requestContent = await postRequest.Content.ReadAsStringAsync();
            _output.WriteLine(requestContent);
            var postResponse = await client.SendAsync(postRequest);
            //postResponse.EnsureSuccessStatusCode();
            var responseString2 = await postResponse.Content.ReadAsStringAsync();
            _output.WriteLine(responseString2);
            Assert.Contains("Mitchell", responseString2);

        }
    }
}