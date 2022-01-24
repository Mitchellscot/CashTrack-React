using AutoMapper;
using CashTrack.Models.ExpenseModels;
using CashTrack.Services.ExpenseService;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using CashTrack.Controllers;

namespace CashTrack.Tests.Controllers
{
    public class ExpenseControllerTests
    {
        private readonly CashTrack.Controllers.ExpenseController _sut;
        public readonly IMapper _mapper;
        public readonly Mock<IExpenseService> _service;

        public ExpenseControllerTests()
        {
            _mapper = Mock.Of<IMapper>();
            _service = new Mock<IExpenseService>();
            _sut = new ExpenseController(_service.Object, _mapper);
        }
        [Fact]
        public async void GetById()
        {
            var result = await _sut.GetAnExpenseById(1);
            var viewResult = Assert.IsType<ActionResult<ExpenseListItem>>(result);
            _service.Verify(s => s.GetExpenseByIdAsync(It.IsAny<int>()), Times.AtLeastOnce());
        }
        [Fact]
        public async void GetAll()
        {
            var request = new ExpenseModels.Request();
            var result = await _sut.GetAllExpenses(request);
            var viewResult = Assert.IsType<ActionResult<ExpenseModels.Response>>(result);
            _service.Verify(s => s.GetExpensesAsync(It.IsAny<ExpenseModels.Request>()), Times.AtLeastOnce());
        }
        [Fact]
        public async void GetByNotes()
        {
            var request = new ExpenseModels.NotesSearchRequest() { SearchTerm = "test" };
            var result = await _sut.GetExpensesByNotes(request);
            var viewResult = Assert.IsType<ActionResult<ExpenseModels.Response>>(result);
            _service.Verify(s => s.GetExpensesByNotesAsync(It.IsAny<ExpenseModels.NotesSearchRequest>()), Times.AtLeastOnce());
        }
        [Fact]
        public async void GetByAmount()
        {
            var request = new ExpenseModels.AmountSearchRequest() { Query = 1.00m };
            var result = await _sut.GetExpensesByAmount(request);
            var viewResult = Assert.IsType<ActionResult<ExpenseModels.Response>>(result);
            _service.Verify(s => s.GetExpensesByAmountAsync(It.IsAny<ExpenseModels.AmountSearchRequest>()), Times.AtLeastOnce());
        }
        [Fact]
        public async void Create()
        {
            var request = new AddEditExpense();
            var result = await _sut.CreateExpense(request);
            var viewResult = Assert.IsType<ActionResult<AddEditExpense>>(result);
            _service.Verify(s => s.CreateExpenseAsync(It.IsAny<AddEditExpense>()), Times.AtLeastOnce());
        }
        [Fact]
        public async void Update()
        {
            var request = new AddEditExpense() { Id = 99999 };
            var result = await _sut.UpdateExpense(request);
            var viewResult = Assert.IsType<ActionResult<AddEditExpense>>(result);
            _service.Verify(s => s.UpdateExpenseAsync(It.IsAny<AddEditExpense>()), Times.AtLeastOnce());
        }
        [Fact]
        public async void Delete()
        {
            var result = await _sut.DeleteExpense(99999);
            _service.Verify(s => s.DeleteExpenseAsync(It.IsAny<int>()), Times.AtLeastOnce());
        }
    }
}
