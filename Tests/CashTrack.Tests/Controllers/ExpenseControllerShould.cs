using AutoMapper;
using CashTrack.Controllers;
using CashTrack.Models.ExpenseModels;
using CashTrack.Services.ExpenseService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
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
        public IExpenseService _service { get; }

        public ExpenseControllerShould(ITestOutputHelper output)
        {
            _output = output;
            _mapper = Mock.Of<IMapper>();
            _logger = Mock.Of<ILogger<ExpenseController>>();
            _service = Mock.Of<IExpenseService>();
            _sut = new ExpenseController(_logger, _service, _mapper);
        }
        [Fact]
        public void ReturnASingleExpenseResponse()
        {
            var result = _sut.GetAnExpenseById(1);
            var viewResult = Assert.IsType<Task<ActionResult<ExpenseListItem>>>(result);
        }
        [Fact]
        public void ReturnsMultipleExpenseResponse()
        {
            var request = new ExpenseModels.Request();
            var result = _sut.GetAllExpenses(request);
            var viewResult = Assert.IsType<Task<ActionResult<ExpenseModels.Response>>>(result);
        }
    }
}
