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
using CashTrack.Data;

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
                .UseEnvironment("Test")
                .UseStartup<CashTrack.Startup>()
                
                ;

            var server = new TestServer(builder);

            var client = server.CreateClient();
            var response = await client.GetAsync("/Expense");

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            _output.WriteLine(responseString);
            //Assert.Contains
        }
    }
}