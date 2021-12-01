using AutoMapper;
using CashTrack.Controllers;
using CashTrack.Models.ExpenseModels;
using CashTrack.Services.ExpenseRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace CashTrack.Tests.Controllers
{
    public class ExpenseControllerShould
    {
        private readonly ExpenseController _sut;
        private readonly ITestOutputHelper _output;

        public IMapper _mapper { get; }

        private readonly ILogger<ExpenseController> _logger;

        public IExpenseService _repository { get; }

        public ExpenseControllerShould(ITestOutputHelper output)
        {
            _output = output;
            _mapper = Mock.Of<IMapper>();
            _logger = Mock.Of<ILogger<ExpenseController>>();
            _repository = Mock.Of<IExpenseService>();
            _sut = new ExpenseController(_logger, _repository, _mapper);
        }
        [Fact]
        public void ReturnASingleExpense()
        {
            var result = _sut.GetAnExpenseById(1);
            var viewResult = Assert.IsType<Task<ActionResult<Expense>>>(result);


        }
    }
}
