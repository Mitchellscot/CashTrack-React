using Microsoft.AspNetCore.Mvc;
using CashTrack.Data.Services.UserRepository;
using Microsoft.AspNetCore.Authorization;
using CashTrack.Data.Entities;
using Microsoft.Extensions.Logging;
using AutoMapper;
using System.Threading.Tasks;
using CashTrack.Services.ExpenseRepository;
using Microsoft.AspNetCore.Http;
using CashTrack.Models.ExpenseModels;
using System;
using CashTrack.Helpers.Exceptions;
using Npgsql;

namespace CashTrack.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class ExpenseController : ControllerBase
    {
        //Install this tool to test:
        //https://docs.microsoft.com/en-us/aspnet/core/tutorials/web-api-help-pages-using-swagger?view=aspnetcore-5.0

        //also, think about how you can reduce the number of controllers here this is rediculous

        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;
        private readonly IExpenseService _expenseService;

        public ExpenseController(ILogger<UserController> logger, IExpenseService expenseService, IMapper mapper)
        {
            this._mapper = mapper;
            this._logger = logger;
            this._expenseService = expenseService;
        }

        [HttpGet]
        public async Task<ActionResult<Expense[]>> GetAllExpenses(int pageNumber = 1, int pageSize = 25)
        {
            try
            {
                var response = await _expenseService.GetExpenses(pageNumber, pageSize);
                return Ok(response);
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
        public async Task<ActionResult<Expense>> GetAnExpenseById(int id)
        {
            try
            {
                var result = await _expenseService.GetExpenseById(id);
                return Ok(_mapper.Map<Expense>(result));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        //api/expense/date/{date}
        //accepts a date in string format
        //Expenses queried by dates, all return collections
        [HttpGet("/date")]
        //create a DateRequest object with just a date
        public async Task<ActionResult<Expense[]>> GetAllExpensesByDate(string date)
        {
            try
            {
                //DateTime.TryParse(date)...
                //logic goes here
                return Content("This is the data you were looking for.");

            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
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

        //    //api/expense/date-range
        //    //accepts two dates in json obj format
        //    //Expenses queried by dates, returns a collection
        //    [HttpGet("/date-range")]
        //    //create a DateRange request
        //    public async Task<ActionResult<ExpeseModel[]>> GetExpensesByDateRange([FromBody] DateRangeRequest dateRange)
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

        //    //api/expense/month
        //    //accepts a datetime and returns all expenses in a given month
        //    //Expenses queried by date, returns a collection
        //    [HttpGet("/month")]
        //    public async Task<ActionResult<ExpeseModel[]>> GetExpensesByMonth([FromBody] DateRequest date)
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

        //    //api/expense/year
        //    //accepts a datetime and returns all expenses in a given year
        //    //Expenses queried by date, returns a collection
        //    [HttpGet("/year")]
        //    public async Task<ActionResult<ExpeseModel[]>> GetExpensesByYear([FromBody] DateRequest date)
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

        //    //api/expense/quarter
        //    //accepts a datetime and returns all expenses in a given year/quarter
        //    //Expenses queried by date, returns a collection
        //    [HttpGet("/quarter")]
        //    //DateRequest contains a parameter that is an int and indicates the quarter. Default is 0.
        //    public async Task<ActionResult<ExpeseModel[]>> GetExpensesByQuarter([FromBody] DateRequest date)
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
        //    //returns transactions that contain a specific word in the notes section
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
