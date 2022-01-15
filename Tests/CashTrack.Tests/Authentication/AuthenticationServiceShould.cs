using AutoMapper;
using CashTrack.Data;
using CashTrack.Data.Entities;
using CashTrack.Helpers;
using CashTrack.IntegrationTests;
using CashTrack.Services.AuthenticationServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.IO;
using Xunit;

namespace CashTrack.Tests.Authentication
{
    public class AuthenticationServiceShould
    {
        private readonly AuthenticationService _authService;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthenticationService> _logger;
        private readonly IOptions<AppSettings> _settings;

        public AuthenticationServiceShould()
        {
            _mapper = Mock.Of<IMapper>();
            _logger = Mock.Of<ILogger<AuthenticationService>>();
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Test.json")
                .Build();
            var settings = configuration.GetSection("AppSettings").Get<AppSettings>();
            _settings = new TestOptionsSnapshot<AppSettings>(settings);
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase("blahblahblah").Options;
            var context = new AppDbContext(options, configuration);
            _authService = new AuthenticationService(context, _mapper, _logger, _settings);
        }
        [Fact]
        public void GenerateJWTToken()
        {
            var henry = new Users()
            {
                id = 1,
                first_name = "Henry",
                last_name = "Scott",
                email = "Henry@example.com",
                password_hash = "blahblahblah"
            };
            var lydia = new Users()
            {
                id = 2,
                first_name = "Lydia",
                last_name = "Scott",
                email = "Lydia@example.com",
                password_hash = "blahblahblah"
            };
            var henryToken = _authService.GenerateJwtToken(henry);
            var henryTokenAgain = _authService.GenerateJwtToken(henry);
            var lydiaToken = _authService.GenerateJwtToken(lydia);
            Assert.NotNull(henryToken);
            Assert.NotNull(henryTokenAgain);
            Assert.NotNull(lydiaToken);
            Assert.Equal(henryToken, henryTokenAgain);
            Assert.NotEqual(henryToken, lydiaToken);
        }
    }
}
