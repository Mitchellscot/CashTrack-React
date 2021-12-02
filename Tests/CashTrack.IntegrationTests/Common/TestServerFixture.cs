using System;
using System.IO;
using System.Net.Http;
using CashTrack.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Bogus;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace CashTrack.IntegrationTests
{
    public class TestServerFixture : IDisposable
    {
        private readonly TestServer _testServer;
        public HttpClient Client { get; }
        public Faker Faker;

        public TestServerFixture()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.test.json")
                .Build();
            var builder = new WebHostBuilder()
                .UseContentRoot(GetContentRootPath())
                .UseEnvironment("Test")
                .UseConfiguration(configuration)
                .ConfigureServices(services =>
                {
                    services.AddDbContext<AppDbContext>(options =>
                    {
                        options.UseNpgsql(configuration.GetConnectionString("TestDB"));
                    });
                })
                .UseStartup<CashTrack.Startup>();
            _testServer = new TestServer(builder);
            Client = _testServer.CreateClient();
            Faker = new Faker();
        }

        private string GetContentRootPath()
        {
            string testProjectPath = PlatformServices.Default.Application.ApplicationBasePath;
            var relativePathToWebProject = @"..\..\..\..\..\src";
            return Path.Combine(testProjectPath, relativePathToWebProject);
        }

        public async Task<HttpResponseMessage> SendPostRequestAsync(string path, object request)
        {
            var message = new HttpRequestMessage(HttpMethod.Post, path)
            {
                Content = new StringContent(
                    JsonConvert.SerializeObject(request),
                    Encoding.UTF8,
                    "application/json")
            };
            return await Client.SendAsync(message);

        }

        public void Dispose()
        {
            Client.Dispose();
            _testServer.Dispose();
        }
    }
}
