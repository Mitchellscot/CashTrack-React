using AutoMapper;
using CashTrack.Controllers;
using CashTrack.Models.UserModels;
using CashTrack.Services.UserService;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace CashTrack.Tests.Controllers
{
    public class UserController
    {
        private readonly Mock<IUserService> _service;
        private readonly CashTrack.Controllers.UserController _sut;

        public UserController()
        {
            _service = new Mock<IUserService>();
            _sut = new CashTrack.Controllers.UserController(_service.Object);
        }
        [Fact]
        public async void ById()
        {
            var result = await _sut.GetUserById(1);
            var viewResult = Assert.IsType<ActionResult<UserModels.Response>>(result);
            _service.Verify(s => s.GetUserByIdAsync(It.IsAny<int>()), Times.AtLeastOnce());

        }
        [Fact]
        public async void GetAll()
        {
            var result = await _sut.GetAllUsers();
            var viewResult = Assert.IsType<ActionResult<UserModels.Response[]>>(result);
            _service.Verify(s => s.GetAllUsersAsync(), Times.AtLeastOnce());
        }
    }
}
