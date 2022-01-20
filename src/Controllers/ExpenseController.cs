using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AutoMapper;
using System.Threading.Tasks;
using CashTrack.Repositories.ExpenseRepository;
using Microsoft.AspNetCore.Http;
using CashTrack.Models.ExpenseModels;
using System;
using CashTrack.Helpers.Exceptions;
using CashTrack.Services.ExpenseService;
using Microsoft.AspNetCore.Routing;

namespace CashTrack.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ExpenseController : ControllerBase
    {
        //Install this tool to test:
        //https://docs.microsoft.com/en-us/aspnet/core/tutorials/web-api-help-pages-using-swagger?view=aspnetcore-5.0

        //also, think about how you can reduce the number of controllers here this is rediculous

        private readonly IMapper _mapper;
        private readonly ILogger<ExpenseController> _logger;
        private readonly IExpenseService _expenseService;

        public ExpenseController(ILogger<ExpenseController> logger, IExpenseService expenseService, IMapper mapper)
        {
            _mapper = mapper;
            _logger = logger;
            _expenseService = expenseService;
        }

        [HttpGet]
        public async Task<ActionResult<ExpenseModels.Response>> GetAllExpenses([FromQuery] ExpenseModels.Request request)
        {
            try
            {
                var response = await _expenseService.GetExpensesAsync(request);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"HEY MITCH - ERROR GETTING ALL EXPENSES {ex.Message}");
                return BadRequest(new { message = ex.Message.ToString() });
            }
        }

        //api/expense/{id}
        //returns one expense
        [HttpGet("{id}")]
        public async Task<ActionResult<ExpenseTransaction>> GetAnExpenseById(int id)
        {
            try
            {
                var result = await _expenseService.GetExpenseByIdAsync(id);
                return Ok(result);
            }
            catch (ExpenseNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        //api/expense/notes/{searchTerm}
        //returns transactions that contain a specific word in the notes section
        [HttpGet("notes")]
        public async Task<ActionResult<ExpenseModels.Response>> GetExpensesByNotes([FromQuery] ExpenseModels.NotesSearchRequest request)
        {
            if (string.IsNullOrEmpty(request.SearchTerm))
            {
                return BadRequest("This endpoint requres a search term");
            }

            try
            {
                var response = await _expenseService.GetExpensesByNotesAsync(request);
                return Ok(response);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        //api/expense/amount?query=25.00
        //returns transactions that contain a specific amount
        [HttpGet("amount")]
        public async Task<ActionResult<ExpenseModels.Response>> GetExpensesByAmount([FromQuery] ExpenseModels.AmountSearchRequest request)
        {
            if (string.IsNullOrEmpty(request.Query.ToString()))
            {
                return BadRequest("This endpoint requres a search term");
            }

            try
            {
                var response = await _expenseService.GetExpensesByAmountAsync(request);
                return Ok(response);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<AddEditExpense>> CreateExpense(AddEditExpense request)
        {
            try
            {
                var result = await _expenseService.CreateUpdateExpenseAsync(request);
                var expense = _mapper.Map<AddEditExpense>(result);
                var location = string.Format("{0}://{1}{2}", Request.Scheme, Request.Host, $"/expense/{expense.Id.Value}");
                return Created(location, expense);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message + ex.InnerException);
            }
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult<AddEditExpense>> UpdateExpense([FromBody] AddEditExpense request)
        {
            if (request.Id == null)
                return BadRequest("Need an Id to update an expense.");

            try
            {
                var result = await _expenseService.CreateUpdateExpenseAsync(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message + ex.InnerException);
            }
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteExpense(int id)
        {
            try
            {
                var result = await _expenseService.DeleteExpenseAsync(id);
                if (!result)
                    return StatusCode(StatusCodes.Status500InternalServerError, "Unable to delete expense");

                return Ok();
            }
            catch (ExpenseNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message + ex.InnerException);
            }
        }

        //    //api/expense/category
        //    //returns transactions from a given category
        //    [HttpGet("/category/{categoryId}")]
        //    public async Task<ActionResult<ExpenseModel[]>> GetcategoryExpenses(int categoryId)
        //    {
        //        try
        //        {
        //            //logic goes here
        //            return Content("This is the data you were looking for.");
        //        }
        //        catch (System.Exception ex)
        //        {
        //            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        //        }
        //    }

        //    //api/expense/category/stats
        //    //Accepts an object that contains a category id
        //    //returns all transactions that match that criteria
        //    [HttpGet("/category/{categoryId}/stats")]
        //    //create a category Request model that has a category Id and a date range object
        //    public async Task<ActionResult<categoryStats>> GetcategoryStats([FromBody] int categoryId)
        //    {
        //        try
        //        {
        //            //logic goes here
        //            return Content("This is the data you were looking for.");
        //        }
        //        catch (System.Exception ex)
        //        {
        //            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        //        }
        //    }


        //    //api/expense/tags/{tagId}
        //    //Accepts a tag id
        //    //returns all transactions that contain that tag Id
        //    [HttpGet("/tags/[tagId]")]
        //    //create a Tag Request model that has a tag Id
        //    public async Task<ActionResult<ExpenseModel[]>> GetExpensesByTag(int tagId)
        //    {
        //        try
        //        {
        //            //logic goes here
        //            return Content("This is the data you were looking for.");
        //        }
        //        catch (System.Exception ex)
        //        {
        //            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        //        }
        //    }

    }
}
