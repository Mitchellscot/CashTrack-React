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
        [HttpGet("{id:int}")]
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

        [HttpPost]
        public async Task<ActionResult<AddEditExpense>> CreateExpense([FromBody] AddEditExpense request)
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

        //    //api/expense/date/total
        //    //accepts a date in string format
        //    //Returns the amount spent that day
        //    [HttpGet("/date/total")]
        //    public async Task<ActionResult<int>> GetExpenseTotalByDate([FromBody] DateRequest date)
        //    {
        //        try
        //        {
        //            //DateTime.TryParse(date)...
        //            //logic goes here
        //            return Content("This is the data you were looking for.");

        //        }
        //        catch (System.Exception ex)
        //        {
        //            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        //        }
        //    }

        //    //api/expense/date-range/total
        //    //accepts two dates in json obj format
        //    //returns the amount of money spent between those two dates
        //    [HttpGet("/date-range/total")]
        //    public async Task<ActionResult<int>> GetExpenseTotalByDateRange([FromBody] DateRangeRequest dateRange)
        //    {
        //        try
        //        {
        //            //DateTime.TryParse(dateRange.BeginDate)...
        //            //logic goes here
        //            return Content("This is the data you were looking for.");

        //        }
        //        catch (System.Exception ex)
        //        {
        //            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        //        }
        //    }

        //    //api/expense/month/total
        //    //accepts a datetime
        //    //returns the amount of money spent that month
        //    [HttpGet("/month/total")]
        //    public async Task<ActionResult<int>> GetExpenseTotalByMonth([FromBody] DateRequest date)
        //    {
        //        try
        //        {
        //            //DateTime.TryParse(date)...
        //            //logic goes here
        //            return Content("This is the data you were looking for.");

        //        }
        //        catch (System.Exception ex)
        //        {
        //            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        //        }
        //    }

        //    //api/expense/year/total
        //    //accepts a datetime
        //    //returns the amount of money spent that year
        //    [HttpGet("/year/total")]
        //    public async Task<ActionResult<int>> GetExpenseTotalByYear([FromBody] DateRequest date)
        //    {
        //        try
        //        {
        //            //DateTime.TryParse(date)...
        //            //logic goes here
        //            return Content("This is the data you were looking for.");

        //        }
        //        catch (System.Exception ex)
        //        {
        //            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        //        }
        //    }

        //    //api/expense/quarter/total
        //    //accepts a datetime and an int indicating the quarter
        //    //returns an int that is the sum amount of money spent that quarter
        //    [HttpGet("/quarter/total")]
        //    //DateRequest contains a parameter that is an int and indicates the quarter. Default is 0.
        //    public async Task<ActionResult<int>> GetExpenseTotalByQuarter([FromBody] DateRequest date)
        //    {
        //        try
        //        {
        //            //DateTime.TryParse(date)...
        //            //logic goes here
        //            return Content("This is the data you were looking for.");

        //        }
        //        catch (System.Exception ex)
        //        {
        //            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        //        }
        //    }

        //    //api/expense/amount
        //    //accepts an amount and returns all expenses that match that amount
        //    //Expenses queried by amount, returns a collection
        //    //Not sure how often this will get used, or where, but it's handy.
        //    [HttpGet("/amount")]
        //    public async Task<ActionResult<ExpeseModel[]>> GetExpensesByAmount([FromBody] int amount)
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

        //    //api/expense/merchant/{merchantId}
        //    //accepts an int indicating the merchant Id
        //    //returns all transactions for that merchant, sorted by date
        //    [HttpGet("/merchant/{merchantId}")]
        //    //might need a custom model for this response instead of the standard ExpenseModel
        //    public async Task<ActionResult<ExpeseModel[]>> GetExpensesByMerchant(int merchantId)
        //    {
        //        try
        //        {
        //            //int.TryParse(amount)...
        //            //logic goes here
        //            return Content("This is the data you were looking for.");
        //        }
        //        catch (System.Exception ex)
        //        {
        //            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        //        }
        //    }

        //    //api/expense/merchant/{merchantId}/total
        //    //accepts an int indicating the merchant Id
        //    //returns an int that is the total amount spent at that merchant
        //    [HttpGet("/merchant/{merchantId}/total")]
        //    //might need a custom model for this response instead of the standard ExpenseModel
        //    public async Task<ActionResult<int>> GetExpenseTotalByMerchant(int merchantId)
        //    {
        //        try
        //        {
        //            //int.TryParse(amount)...
        //            //logic goes here
        //            return Content("This is the data you were looking for.");

        //        }
        //        catch (System.Exception ex)
        //        {
        //            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        //        }
        //    }

        //    //api/expense/merchant/{merchantId}/total/date-range
        //    //accepts an object containing an int indicating the merchant Id and a beginDate and endDate
        //    //returns an int that is the total amount spent at that merchant during a given date range
        //    [HttpGet("/merchant/{merchantId}/date-range/total")]
        //    //might need a custom model for this response instead of the standard ExpenseModel
        //    //create a new model for this request
        //    public async Task<ActionResult<int>> GetMerchantExpenseTotalByDateRange(MerchantDateRangeRequest merchantId)
        //    {
        //        try
        //        {
        //            //int.TryParse(amount)...
        //            //logic goes here
        //            return Content("This is the data you were looking for.");

        //        }
        //        catch (System.Exception ex)
        //        {
        //            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        //        }
        //    }

        //    //api/expense/merchant/date-range/{merchantId}
        //    //accepts an int indicating the merchant Id and a date range object
        //    //returns all transactions for that merchant within the given date range
        //    [HttpGet("/merchant/date-range/{merchantId}")]
        //    //might need a custom request model... merchantId, beginDate, endDate
        //    //might need a custom model for this response as well
        //    //CONSIDER doing a query by location as well, though that functionality would require more work...
        //    public async Task<ActionResult<ExpeseModel[]>> GetMerchantExpensesByDateRange([FromBody] MerchantDateRangeRequest merchantId)
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

        //    //api/expense/notes/{searchTerm}
        //    //returns transactions that contain a specific word in the notes section
        //    [HttpGet("/notes/{searchTerm}")]
        //    public async Task<ActionResult<ExpenseModel[]>> GetExpensesByNotes(string searchTerm)
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

        //    //api/expense/category/date-range
        //    //Accepts an object that contains a category id and a beginDate and endDate
        //    //returns all transactions that match that criteria
        //    [HttpGet("/category/{categoryId}/date-range")]
        //    //create a category Request model that has a category Id and a date range object
        //    public async Task<ActionResult<ExpenseModel[]>> GetcategoryExpensesByDateRange([FromBody] categoryRequest categoryRequest)
        //    {
        //        //just make one controller and have the client send the date, month, year, daterange, etc...
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

        //    //api/expense/main-category/{maincategoryId}
        //    //Accepts an object that contains a category id and a beginDate and endDate
        //    //returns all transactions that match that criteria
        //    [HttpGet("/main-category/{maincategoryId}")]
        //    //Use the categoryRequest object for this controller as well, as they are both ints and accept the same parameters
        //    public async Task<ActionResult<ExpenseModel[]>> GetExpensesBycategory([FromBody] categoryRequest categoryRequest)
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

        //    //api/expense/main-category/stats
        //    //Accepts an object that contains a category id
        //    //returns all transactions that match that criteria
        //    [HttpGet("/main-category/{maincategoryId}/stats")]
        //    //create a category Request model that has a category Id and a date range object
        //    public async Task<ActionResult<categoryStats>> GetMaincategoryStats(int categoryId)
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

        //    //api/merchant/stats
        //    //returns a bunch of cool stats about my shopping habbits
        //    [HttpGet("/merchant/stats")]
        //    //Most shopped at stores... by category
        //    //amount spent at the stores
        //    //Average amount spent at the stores (table broken up by year?)
        //    //Think this one through, LOW PRIORITY
        //    //definitely need to create a custom return object for this one
        //    //might belong in the Merchant Controller??
        //    public async Task<ActionResult<MerchantStats>> GetMerchantStats()
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
