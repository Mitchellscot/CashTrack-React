﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CashTrack.Controllers;
using CashTrack.Models.AuthenticationModels;
using CashTrack.Models.UserModels;
using CashTrack.Repositories.UserRepository;
using CashTrack.Services.AuthenticationServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace CashTrack.Tests.Controllers
{
    public class UserControllerShould
    {
        private readonly ITestOutputHelper _output;
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;
        private readonly IAuthenticationService _service;
        private readonly AuthenticateController _controller;

        public UserControllerShould(ITestOutputHelper output)
        {
            _output = output;
            _mapper = Mock.Of<IMapper>();
            _logger = Mock.Of<ILogger<UserController>>();
            _service = Mock.Of<IAuthenticationService>();
            _controller = new AuthenticateController(_logger, _service, _mapper);
        }
        [Fact]
        public void SignUserIn()
        {
            var request = new Authentication.Request("mitchell", "password");
            var result = _controller.Authenticate(request);
            var viewResult = Assert.IsType<Task<ActionResult<Authentication.Response>>>(result);
            
        }
        //[Fact]
        //public void ReturnsInvalidPassword()
        //{
        //    var request = new Authentication.Request("Mitchell", "passwordd");
        //    var result = _controller.Authenticate(request);

        //}
    }
}